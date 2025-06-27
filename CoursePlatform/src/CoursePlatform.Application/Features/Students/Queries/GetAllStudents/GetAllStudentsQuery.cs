using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Students.Queries.GetAllStudents;

public record GetAllStudentsQuery : IRequest<IEnumerable<StudentResponseDTO>>;
