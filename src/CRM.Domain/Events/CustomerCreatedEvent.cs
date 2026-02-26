namespace CRM.Domain.Events;

using CRM.Domain.Core;

public sealed class CustomerCreatedEvent : DomainEvent
{
    public string Name { get; }

    public CustomerCreatedEvent(Guid aggregateId, string name) : base(aggregateId)
    {
        Name = name;
    }
}