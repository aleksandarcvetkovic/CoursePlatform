using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Students.Queries.GetAllStudents;

public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<StudentResponseDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllStudentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StudentResponseDTO>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var students = await _context.Students.ToListAsync(cancellationToken);
        return students.ToStudentResponseDTOs();
    }
}
