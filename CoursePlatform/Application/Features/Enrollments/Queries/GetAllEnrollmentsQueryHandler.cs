using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Enrollments.Queries.GetAllEnrollments;

public class GetAllEnrollmentsQueryHandler : IRequestHandler<GetAllEnrollmentsQuery, IEnumerable<EnrollmentResponseDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllEnrollmentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EnrollmentResponseDTO>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var enrollments = await _context.Enrollments.ToListAsync(cancellationToken);
        return enrollments.ToEnrollmentResponseDTOs();
    }
}
