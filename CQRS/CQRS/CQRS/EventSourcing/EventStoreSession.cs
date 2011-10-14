﻿#region usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Transactions;
using Composable.CQRS.EventSourcing.SQLServer;
using Composable.ServiceBus;
using Composable.StuffThatDoesNotBelongHere;
using Composable.System;
using Composable.System.Linq;
using Composable.SystemExtensions.Threading;
using Composable.UnitsOfWork;
using log4net;

#endregion

namespace Composable.CQRS.EventSourcing
{
    public class EventStoreSession : IEventStoreSession, IUnitOfWorkParticipantWhoseCommitMayTriggerChangesInOtherParticipantsMustImplementIdemponentCommit
    {
        private readonly IServiceBus _bus;
        private readonly IEventSomethingOrOther _storage;
        private static ILog Log = LogManager.GetLogger(typeof(EventStoreSession));
        private readonly IDictionary<Guid, IEventStored> _idMap = new Dictionary<Guid, IEventStored>();
        private readonly HashSet<Guid> _publishedEvents = new HashSet<Guid>();
        private readonly SingleThreadedUseGuard _threadGuard;

        public EventStoreSession(IServiceBus bus, IEventSomethingOrOther storage)
        {
            _threadGuard = new SingleThreadedUseGuard(this);
            _bus = bus;
            _storage = storage;
        }

        public TAggregate Get<TAggregate>(Guid aggregateId) where TAggregate : IEventStored
        {
            _threadGuard.AssertNoThreadChangeOccurred();
            TAggregate result;
            if (!DoTryGet(aggregateId, out result))
                ThrowAggregateMissingException(aggregateId);
            return result;
        }

        public bool TryGet<TAggregate>(Guid aggregateId, out TAggregate aggregate) where TAggregate : IEventStored
        {
            _threadGuard.AssertNoThreadChangeOccurred();
            return DoTryGet(aggregateId, out aggregate);
        }

        public TAggregate LoadSpecificVersion<TAggregate>(Guid aggregateId, int version) where TAggregate : IEventStored
        {
            _threadGuard.AssertNoThreadChangeOccurred();
            var aggregate = Activator.CreateInstance<TAggregate>();
            var history = GetHistory(aggregateId);
            if (history.None())
                ThrowAggregateMissingException(aggregateId);
            aggregate.LoadFromHistory(history.Where(e => e.AggregateRootVersion <= version));
            return aggregate;
        }

        public void Save<TAggregate>(TAggregate aggregate) where TAggregate : IEventStored
        {
            _threadGuard.AssertNoThreadChangeOccurred();
            var changes = aggregate.GetChanges().ToList();
            if(aggregate.Version > 0 && changes.None() || changes.Any() && changes.Min(e => e.AggregateRootVersion) > 1)
            {
                throw new AttemptToSaveAlreadyPersistedAggregateException(aggregate);
            }
            if(aggregate.Version == 0 && changes.None())
            {
                throw new AttemptToSaveEmptyAggregate(aggregate);
            }
            _idMap.Add(aggregate.Id, aggregate);
        }

        public void SaveChanges()
        {
            _threadGuard.AssertNoThreadChangeOccurred();
            if(_unitOfWork == null)
            {                
                InternalSaveChanges();
            }else
            {
                var newEvents = _idMap.SelectMany(p => p.Value.GetChanges()).ToList();
                PublishUnpublishedEvents(newEvents);
                Log.DebugFormat("{0} ignored call to SaveChanges since participating in a unit of work", _id);
            }
        }

        public void Dispose()
        {
            _threadGuard.AssertNoThreadChangeOccurred();
            _storage.Dispose();
        }


        public override string ToString()
        {
            return "{0}: {1}".FormatWith(_id, GetType().FullName);
        }

        #region Implementation of IUnitOfWorkParticipant

        private IUnitOfWork _unitOfWork;
        private readonly Guid _id = Guid.NewGuid();

        IUnitOfWork IUnitOfWorkParticipant.UnitOfWork { get { return _unitOfWork; } }
        Guid IUnitOfWorkParticipant.Id { get { return _id; } }

        void IUnitOfWorkParticipant.Join(IUnitOfWork unit)
        {
            _threadGuard.AssertNoThreadChangeOccurred();
            if (_unitOfWork != null)
            {
                throw new ReuseOfEventStoreSessionException(_unitOfWork, unit);
            }
            _unitOfWork = unit;
        }

        void IUnitOfWorkParticipant.Commit(IUnitOfWork unit)
        {
            _threadGuard.AssertNoThreadChangeOccurred();
            ((IUnitOfWorkParticipantWhoseCommitMayTriggerChangesInOtherParticipantsMustImplementIdemponentCommit)this).CommitAndReportIfCommitMayHaveCausedChangesInOtherParticipantsExpectAnotherCommitSoDoNotLeaveUnitOfWork();
        }

        void IUnitOfWorkParticipant.Rollback(IUnitOfWork unit)
        {
            _threadGuard.AssertNoThreadChangeOccurred();
        }

        bool IUnitOfWorkParticipantWhoseCommitMayTriggerChangesInOtherParticipantsMustImplementIdemponentCommit.CommitAndReportIfCommitMayHaveCausedChangesInOtherParticipantsExpectAnotherCommitSoDoNotLeaveUnitOfWork()
        {
            _threadGuard.AssertNoThreadChangeOccurred();
            return InternalSaveChanges();
        }

        #endregion

        private IEnumerable<IAggregateRootEvent> GetHistory(Guid aggregateId)
        {
            var history = _storage.GetHistoryUnSafe(aggregateId);

            int version = 1;
            foreach (var aggregateRootEvent in history)
            {
                if (aggregateRootEvent.AggregateRootVersion != version++)
                {
                    throw new InvalidHistoryException();
                }
            }

            return history;
        }

        private void ThrowAggregateMissingException(Guid aggregateId)
        {
            throw new Exception(string.Format("Aggregate root with Id: {0} not found", aggregateId));
        }

        private bool DoTryGet<TAggregate>(Guid aggregateId, out TAggregate aggregate) where TAggregate : IEventStored
        {
            IEventStored es;
            if (_idMap.TryGetValue(aggregateId, out es))
            {
                aggregate = (TAggregate)es;
                return true;
            }

            var history = GetHistory(aggregateId);
            if (history.Any())
            {
                aggregate = Activator.CreateInstance<TAggregate>();
                aggregate.LoadFromHistory(GetHistory(aggregateId));
                _idMap.Add(aggregateId, aggregate);
                return true;
            }
            else
            {
                aggregate = default(TAggregate);
                return false;
            }
        }

        private void PublishUnpublishedEvents(IEnumerable<IAggregateRootEvent> events)
        {
            var unpublishedEvents = events.Where(e => !_publishedEvents.Contains(e.EventId))
                                          .ToList();
            _publishedEvents.AddRange(unpublishedEvents.Select(e => e.EventId));
            unpublishedEvents.ForEach(_bus.Publish);
        }

        private bool InternalSaveChanges()
        {
            Log.DebugFormat("{0} saving changes with {1} changes from transaction within unit of work {2}", _id, _idMap.Count, _unitOfWork ?? (object)"null");

            var aggregates = _idMap.Select(p => p.Value).ToList();
            var newEvents = aggregates.SelectMany(a => a.GetChanges()).ToList();
            aggregates.ForEach(a => a.AcceptChanges());
            _storage.SaveEvents(newEvents);
            PublishUnpublishedEvents(newEvents);

            return newEvents.Any();
        }
    }
}