using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Commands;

public record CreateCourseCommand(CourseRequestDTO Course) : IRequest<CourseResponseDTO>;
