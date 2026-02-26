namespace CRM.Domain.Core;

public abstract class DomainEvent
{
    public Guid AggregateId { get; set; }
    public DateTime OccurredAt { get; set; }
    public string EventType { get; set; }

    protected DomainEvent(Guid aggregateId)
    {
        AggregateId = aggregateId;
        OccurredAt = DateTime.UtcNow;
        EventType = GetType().Name;
    }
}