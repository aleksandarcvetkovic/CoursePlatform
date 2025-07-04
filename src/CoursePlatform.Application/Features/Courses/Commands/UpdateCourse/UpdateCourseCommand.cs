using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Commands;

public record UpdateCourseCommand(string Id, CourseRequestDTO Course) : IRequest<CourseResponseDTO>;
