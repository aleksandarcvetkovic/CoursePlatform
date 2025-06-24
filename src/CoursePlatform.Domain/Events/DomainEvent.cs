using CoursePlatform.Domain.Common;

namespace CoursePlatform.Domain.Events;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid Id { get; init; } = Guid.NewGuid();
}
