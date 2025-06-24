using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Students.Queries.GetStudentById;

public record GetStudentByIdQuery(string Id) : IRequest<StudentResponseDTO?>;
