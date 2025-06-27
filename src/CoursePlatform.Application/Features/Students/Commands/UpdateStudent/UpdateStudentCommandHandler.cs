using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Students.Commands.UpdateStudent;

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, StudentResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStudentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentResponseDTO> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(request.Id, cancellationToken);

        if (student == null)
            throw new NotFoundException($"Student with ID '{request.Id}' was not found.");

        student.UpdateInfo(request.Student.Name, request.Student.Email);
        
        await _unitOfWork.Students.UpdateAsync(student, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return student.ToStudentResponseDTO();
    }
}
