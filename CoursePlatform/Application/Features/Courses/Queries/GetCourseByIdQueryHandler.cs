using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Courses.Queries.GetCourseById;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseResponseDTO?>
{
    private readonly IApplicationDbContext _context;

    public GetCourseByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseResponseDTO?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
            throw new NotFoundException($"Course with ID '{request.Id}' was not found.");

        return new CourseResponseDTO
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            InstructorId = course.InstructorId
        };
    }
}
