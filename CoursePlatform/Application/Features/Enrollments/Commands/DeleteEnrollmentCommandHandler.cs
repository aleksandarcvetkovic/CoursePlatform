using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Features.Enrollments.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Enrollments.Commands;

public class DeleteEnrollmentCommandHandler : IRequestHandler<DeleteEnrollmentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteEnrollmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (enrollment == null)
        {
            throw new NotFoundException($"Enrollment with ID {request.Id} not found.");
        }

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
