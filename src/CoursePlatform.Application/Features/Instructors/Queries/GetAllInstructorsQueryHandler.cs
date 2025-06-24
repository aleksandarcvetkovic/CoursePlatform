using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Instructors.Queries.GetAllInstructors;

public class GetAllInstructorsQueryHandler : IRequestHandler<GetAllInstructorsQuery, IEnumerable<InstructorResponseDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllInstructorsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InstructorResponseDTO>> Handle(GetAllInstructorsQuery request, CancellationToken cancellationToken)
    {
        var instructors = await _context.Instructors.ToListAsync(cancellationToken);
        return instructors.ToInstructorResponseDTOs();
    }
}
