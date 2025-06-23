using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Students.Queries.GetStudentWithEnrollments;

public class GetStudentWithEnrollmentsQueryHandler : IRequestHandler<GetStudentWithEnrollmentsQuery, StudentWithEnrollmentsDTO?>
{
    private readonly IApplicationDbContext _context;

    public GetStudentWithEnrollmentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentWithEnrollmentsDTO?> Handle(GetStudentWithEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (student == null)
            throw new NotFoundException($"Student with ID '{request.Id}' was not found.");

        return student.ToStudentWithEnrollmentsDTO();
    }
}
