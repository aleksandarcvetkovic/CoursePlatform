using MediatR;

namespace CoursePlatform.Application.Features.Students.Commands.DeleteStudent;

public record DeleteStudentCommand(string Id) : IRequest;
