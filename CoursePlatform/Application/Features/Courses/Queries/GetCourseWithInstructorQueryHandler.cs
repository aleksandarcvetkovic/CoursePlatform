using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Courses.Queries.GetCourseWithInstructor;

public class GetCourseWithInstructorQueryHandler : IRequestHandler<GetCourseWithInstructorQuery, CourseWithInstructorDTO?>
{
    private readonly IApplicationDbContext _context;

    public GetCourseWithInstructorQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseWithInstructorDTO?> Handle(GetCourseWithInstructorQuery request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .Include(c => c.Instructor)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
            throw new NotFoundException($"Course with ID '{request.Id}' was not found.");

        return course.ToCourseWithInstructorDTO();
    }
}
