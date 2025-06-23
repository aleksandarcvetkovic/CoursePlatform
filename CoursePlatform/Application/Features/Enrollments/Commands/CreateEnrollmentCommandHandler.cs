using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Enrollments.Commands.CreateEnrollment;

public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, EnrollmentResponseDTO>
{
    private readonly IApplicationDbContext _context;

    public CreateEnrollmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EnrollmentResponseDTO> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var enrollment = request.Enrollment.ToEnrollment();

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync(cancellationToken);

        return enrollment.ToEnrollmentResponseDTO();
    }
}
