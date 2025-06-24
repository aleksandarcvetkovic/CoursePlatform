using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Students.Commands.DeleteStudent;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteStudentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(request.Id, cancellationToken);

        if (student == null)
            throw new NotFoundException($"Student with ID '{request.Id}' was not found.");

        await _unitOfWork.Students.DeleteAsync(student, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
