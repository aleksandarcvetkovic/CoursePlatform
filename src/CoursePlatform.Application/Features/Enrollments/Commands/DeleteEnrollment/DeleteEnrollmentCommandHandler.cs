using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Features.Enrollments.Commands;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Enrollments.Commands;

public class DeleteEnrollmentCommandHandler : IRequestHandler<DeleteEnrollmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEnrollmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(request.Id, cancellationToken);

        if (enrollment == null)
        {
            throw new NotFoundException($"Enrollment with ID {request.Id} not found.");
        }        await _unitOfWork.Enrollments.DeleteAsync(enrollment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
