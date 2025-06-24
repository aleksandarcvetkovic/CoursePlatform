namespace CoursePlatform.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    Guid Id { get; }
}
