using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Queries.GetCourseById;

public record GetCourseByIdQuery(string Id) : IRequest<CourseResponseDTO?>;
