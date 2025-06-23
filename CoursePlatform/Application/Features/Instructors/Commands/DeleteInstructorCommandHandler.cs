using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Features.Instructors.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Instructors.Commands;

public class DeleteInstructorCommandHandler : IRequestHandler<DeleteInstructorCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteInstructorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _context.Instructors
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (instructor == null)
        {
            throw new NotFoundException($"Instructor with ID {request.Id} not found.");
        }

        _context.Instructors.Remove(instructor);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
