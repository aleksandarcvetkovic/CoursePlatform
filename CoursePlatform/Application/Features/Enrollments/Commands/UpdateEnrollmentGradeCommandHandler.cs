using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Features.Enrollments.Commands.UpdateEnrollmentGrade;

public class UpdateEnrollmentGradeCommandHandler : IRequestHandler<UpdateEnrollmentGradeCommand, EnrollmentResponseDTO>
{
    private readonly IApplicationDbContext _context;

    public UpdateEnrollmentGradeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EnrollmentResponseDTO> Handle(UpdateEnrollmentGradeCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (enrollment == null)
            throw new NotFoundException($"Enrollment with ID '{request.Id}' was not found.");

        enrollment.UpdateGrade(request.GradeRequest.Grade);

        await _context.SaveChangesAsync(cancellationToken);

        return enrollment.ToEnrollmentResponseDTO();
    }
}
