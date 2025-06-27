using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Queries;

public record GetCourseWithInstructorQuery(string Id) : IRequest<CourseWithInstructorDTO?>;
