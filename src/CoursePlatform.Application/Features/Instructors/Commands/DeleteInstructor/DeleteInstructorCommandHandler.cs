using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Features.Instructors.Commands;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Commands;

public class DeleteInstructorCommandHandler : IRequestHandler<DeleteInstructorCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteInstructorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _unitOfWork.Instructors.GetByIdAsync(request.Id, cancellationToken);

        if (instructor == null)
        {
            throw new NotFoundException($"Instructor with ID {request.Id} not found.");
        }        await _unitOfWork.Instructors.DeleteAsync(instructor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
