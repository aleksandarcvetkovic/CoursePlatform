using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Queries;

public record GetCourseByIdQuery(string Id) : IRequest<CourseResponseDTO?>;
