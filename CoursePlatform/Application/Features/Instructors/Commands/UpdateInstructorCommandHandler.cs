using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Features.Instructors.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Instructors.Commands;

public class UpdateInstructorCommandHandler : IRequestHandler<UpdateInstructorCommand, InstructorResponseDTO>
{
    private readonly IApplicationDbContext _context;

    public UpdateInstructorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InstructorResponseDTO> Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _context.Instructors
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);        if (instructor == null)
        {
            throw new NotFoundException($"Instructor with ID {request.Id} not found.");
        }

        instructor.Name = request.InstructorRequest.Name;
        instructor.Email = request.InstructorRequest.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return instructor.ToInstructorResponseDTO();
    }
}
