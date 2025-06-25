using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Queries;

public record GetInstructorWithCoursesQuery(string Id) : IRequest<InstructorWithCoursesDTO>;
