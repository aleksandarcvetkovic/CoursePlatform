using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Enrollments.Commands.UpdateEnrollmentGrade;

public class UpdateEnrollmentGradeCommandHandler : IRequestHandler<UpdateEnrollmentGradeCommand, EnrollmentResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEnrollmentGradeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EnrollmentResponseDTO> Handle(UpdateEnrollmentGradeCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(request.Id, cancellationToken);

        if (enrollment == null)
            throw new NotFoundException($"Enrollment with ID '{request.Id}' was not found.");

        enrollment.UpdateGrade(request.GradeRequest.Grade);

        await _unitOfWork.Enrollments.UpdateAsync(enrollment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return enrollment.ToEnrollmentResponseDTO();
    }
}
