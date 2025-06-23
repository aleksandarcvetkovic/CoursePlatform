using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Commands.CreateCourse;

public record CreateCourseCommand(CourseRequestDTO Course) : IRequest<CourseResponseDTO>;
