﻿using System;
using System.Collections.Generic;
using Composable.CQRS.EventHandling;
using Composable.System.Linq;

namespace Composable.CQRS.EventSourcing
{
    public abstract class SelfUpdatingSingleAggregateQueryModel<TRootQueryModel, TAggregateRootBaseEventInterface>
        where TRootQueryModel : SelfUpdatingSingleAggregateQueryModel<TRootQueryModel, TAggregateRootBaseEventInterface>
    {
        readonly CallMatchingHandlersInRegistrationOrderEventDispatcher<TAggregateRootBaseEventInterface> _eventAppliersEventDispatcher =
            new CallMatchingHandlersInRegistrationOrderEventDispatcher<TAggregateRootBaseEventInterface>();


        public Guid Id { get; private set; }

        protected SelfUpdatingSingleAggregateQueryModel()
        {
            RegisterEventAppliers()
                .ForGenericEvent<IAggregateRootCreatedEvent>(e => Id = e.AggregateRootId);
        }

        public void ApplyEvent(TAggregateRootBaseEventInterface @event) { _eventAppliersEventDispatcher.Dispatch(@event); }
        public void ApplyEvents(IEnumerable<TAggregateRootBaseEventInterface> @event)
        {
            @event.ForEach(_eventAppliersEventDispatcher.Dispatch);
        }

        protected CallMatchingHandlersInRegistrationOrderEventDispatcher<TAggregateRootBaseEventInterface>.RegistrationBuilder RegisterEventAppliers()
        {
            return _eventAppliersEventDispatcher.RegisterHandlers();
        }


        public abstract class Component<TComponent, TComponentBaseEventInterface, TComponentCreatedEventInterface>
            where TComponentBaseEventInterface : TAggregateRootBaseEventInterface, IAggregateRootComponentEvent
            where TComponentCreatedEventInterface : TComponentBaseEventInterface, IAggregateRootEntityCreatedEvent
            where TComponent : Component<TComponent, TComponentBaseEventInterface, TComponentCreatedEventInterface>
        {
            private readonly CallMatchingHandlersInRegistrationOrderEventDispatcher<TComponentBaseEventInterface> _eventAppliersEventDispatcher =
                new CallMatchingHandlersInRegistrationOrderEventDispatcher<TComponentBaseEventInterface>();


            protected TRootQueryModel RootQueryModel { get; private set; }

            private void ApplyEvent(TComponentBaseEventInterface @event) { _eventAppliersEventDispatcher.Dispatch(@event); }

            protected CallMatchingHandlersInRegistrationOrderEventDispatcher<TComponentBaseEventInterface>.RegistrationBuilder RegisterEventAppliers()
            {
                return _eventAppliersEventDispatcher.RegisterHandlers();
            }

            public static IQueryModelComponentCollection<TComponent> CreateSelfManagingCollection(TRootQueryModel rootQueryModel) => new Collection(rootQueryModel);

            public class Collection : IQueryModelComponentCollection<TComponent>
            {
                private readonly TRootQueryModel _aggregate;
                public Collection(TRootQueryModel aggregate)
                {
                    _aggregate = aggregate;
                    _aggregate.RegisterEventAppliers()
                         .For<TComponentCreatedEventInterface>(
                            e =>
                            {
                                var component = (TComponent)Activator.CreateInstance(typeof(TComponent), nonPublic:true);
                                component.RootQueryModel = _aggregate;

                                _components.Add(e.EntityId, component);
                                _componentsInCreationOrder.Add(component);
                            })
                        .For<TComponentBaseEventInterface>(e => _components[e.EntityId].ApplyEvent(e));
                }


                public IReadOnlyList<TComponent> InCreationOrder => _componentsInCreationOrder;

                public bool TryGet(Guid id, out TComponent component) => _components.TryGetValue(id, out component);
                public bool Exists(Guid id) => _components.ContainsKey(id);
                public TComponent Get(Guid id) => _components[id];

                private readonly Dictionary<Guid, TComponent> _components = new Dictionary<Guid, TComponent>();
                private readonly List<TComponent> _componentsInCreationOrder = new List<TComponent>();
            }
        }
    }
}
