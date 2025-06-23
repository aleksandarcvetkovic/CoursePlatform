using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Commands.CreateCourse;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseResponseDTO>
{
    private readonly IApplicationDbContext _context;

    public CreateCourseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseResponseDTO> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = request.Course.ToCourse();

        _context.Courses.Add(course);
        await _context.SaveChangesAsync(cancellationToken);

        return course.ToCourseResponseDTO();
    }
}
