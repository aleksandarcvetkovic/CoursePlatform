using MediatR;

namespace CoursePlatform.Application.Features.Courses.Commands;

public record DeleteCourseCommand(string Id) : IRequest;
