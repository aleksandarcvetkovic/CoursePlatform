using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Commands;

public record DeleteInstructorCommand(string Id) : IRequest;
