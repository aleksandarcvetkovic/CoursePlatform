using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Students.Commands.UpdateStudent;

public record UpdateStudentCommand(string Id, StudentRequestDTO Student) : IRequest<StudentResponseDTO>;
