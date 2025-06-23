using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Courses.Commands.UpdateCourse;

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, CourseResponseDTO>
{
    private readonly IApplicationDbContext _context;

    public UpdateCourseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseResponseDTO> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
            throw new NotFoundException($"Course with ID '{request.Id}' was not found.");

        course.Title = request.Course.Title;
        course.Description = request.Course.Description;
        course.InstructorId = request.Course.InstructorId;

        await _context.SaveChangesAsync(cancellationToken);

        return new CourseResponseDTO
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            InstructorId = course.InstructorId
        };
    }
}
