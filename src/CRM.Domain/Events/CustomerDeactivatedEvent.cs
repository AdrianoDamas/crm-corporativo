namespace CRM.Domain.Events;

using CRM.Domain.Core;

public sealed class CustomerDeactivatedEvent : DomainEvent
{
    public CustomerDeactivatedEvent(Guid aggregateId) : base(aggregateId) { }
}