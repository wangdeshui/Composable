using System;
using Composable.DDD;
using Composable.DomainEvents;

namespace Composable.CQRS.Command
{
    public class CommandSucceededEvent : ValueObject<CommandSucceededEvent>, IDomainEvent
    {
        public Guid CommandId { get; set; }
    }
}