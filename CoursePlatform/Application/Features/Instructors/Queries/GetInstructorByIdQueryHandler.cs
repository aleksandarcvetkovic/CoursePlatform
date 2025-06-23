using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Instructors.Queries.GetInstructorById;

public class GetInstructorByIdQueryHandler : IRequestHandler<GetInstructorByIdQuery, InstructorResponseDTO?>
{
    private readonly IApplicationDbContext _context;

    public GetInstructorByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InstructorResponseDTO?> Handle(GetInstructorByIdQuery request, CancellationToken cancellationToken)
    {
        var instructor = await _context.Instructors
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (instructor == null)
            throw new NotFoundException($"Instructor with ID '{request.Id}' was not found.");

        return instructor.ToInstructorResponseDTO();
    }
}
