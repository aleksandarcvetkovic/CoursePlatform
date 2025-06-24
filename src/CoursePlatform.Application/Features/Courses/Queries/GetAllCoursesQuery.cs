using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Queries.GetAllCourses;

public record GetAllCoursesQuery : IRequest<IEnumerable<CourseResponseDTO>>;
