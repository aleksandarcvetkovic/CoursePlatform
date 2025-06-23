using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Courses.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, IEnumerable<CourseResponseDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllCoursesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseResponseDTO>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _context.Courses.ToListAsync(cancellationToken);
        return courses.ToCourseResponseDTOs();
    }
}
