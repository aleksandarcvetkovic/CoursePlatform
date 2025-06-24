using CoursePlatform.Domain.Events;

namespace CoursePlatform.Domain.Events.Students;

public record StudentCreatedEvent(string StudentId, string Name, string Email) : DomainEvent;
