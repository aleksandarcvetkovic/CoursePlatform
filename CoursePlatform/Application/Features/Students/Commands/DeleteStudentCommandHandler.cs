using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Students.Commands.DeleteStudent;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (student == null)
            throw new NotFoundException($"Student with ID '{request.Id}' was not found.");

        _context.Students.Remove(student);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
