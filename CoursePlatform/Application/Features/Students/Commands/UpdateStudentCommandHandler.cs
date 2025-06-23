using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Students.Commands.UpdateStudent;

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, StudentResponseDTO>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentResponseDTO> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (student == null)
            throw new NotFoundException($"Student with ID '{request.Id}' was not found.");

        student.UpdateFromDTO(request.Student);

        await _context.SaveChangesAsync(cancellationToken);

        return student.ToStudentResponseDTO();
    }
}
