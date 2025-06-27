using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Students.Queries.GetStudentWithEnrollments;

public record GetStudentWithEnrollmentsQuery(string Id) : IRequest<StudentWithEnrollmentsDTO?>;
