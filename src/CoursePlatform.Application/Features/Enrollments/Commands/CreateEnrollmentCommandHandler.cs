using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Enrollments.Commands.CreateEnrollment;

public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, EnrollmentResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateEnrollmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EnrollmentResponseDTO> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var enrollment = Enrollment.Create(request.Enrollment.StudentId, request.Enrollment.CourseId);

        await _unitOfWork.Enrollments.AddAsync(enrollment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return enrollment.ToEnrollmentResponseDTO();
    }
}
