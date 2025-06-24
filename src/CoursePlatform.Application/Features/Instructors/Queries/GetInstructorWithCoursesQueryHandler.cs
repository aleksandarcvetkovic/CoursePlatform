using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Features.Instructors.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Instructors.Queries;

public class GetInstructorWithCoursesQueryHandler : IRequestHandler<GetInstructorWithCoursesQuery, InstructorWithCoursesDTO>
{
    private readonly IApplicationDbContext _context;

    public GetInstructorWithCoursesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InstructorWithCoursesDTO> Handle(GetInstructorWithCoursesQuery request, CancellationToken cancellationToken)
    {        var instructor = await _context.Instructors
            .Include(i => i.Courses)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (instructor == null)
        {
            throw new NotFoundException($"Instructor with ID {request.Id} not found.");
        }

        return instructor.ToInstructorWithCoursesDTO();
    }
}
