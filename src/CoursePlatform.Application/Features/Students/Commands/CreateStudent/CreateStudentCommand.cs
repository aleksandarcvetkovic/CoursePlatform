using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Students.Commands.CreateStudent;

public record CreateStudentCommand(StudentRequestDTO Student) : IRequest<StudentResponseDTO>;
