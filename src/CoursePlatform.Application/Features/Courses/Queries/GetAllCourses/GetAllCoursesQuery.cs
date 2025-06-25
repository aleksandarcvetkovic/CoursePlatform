using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Queries;

public record GetAllCoursesQuery : IRequest<IEnumerable<CourseResponseDTO>>;
