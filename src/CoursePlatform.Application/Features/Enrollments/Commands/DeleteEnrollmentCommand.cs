using MediatR;

namespace CoursePlatform.Application.Features.Enrollments.Commands;

public record DeleteEnrollmentCommand(string Id) : IRequest;
