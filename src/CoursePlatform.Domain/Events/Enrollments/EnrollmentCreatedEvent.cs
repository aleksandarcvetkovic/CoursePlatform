using CoursePlatform.Domain.Events;

namespace CoursePlatform.Domain.Events.Enrollments;

public record EnrollmentCreatedEvent(string EnrollmentId, string StudentId, string CourseId) : DomainEvent;
