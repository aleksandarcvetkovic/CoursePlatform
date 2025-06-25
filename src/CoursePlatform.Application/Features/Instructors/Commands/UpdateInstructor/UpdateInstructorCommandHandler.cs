using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Features.Instructors.Commands;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Commands;

public class UpdateInstructorCommandHandler : IRequestHandler<UpdateInstructorCommand, InstructorResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateInstructorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InstructorResponseDTO> Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _unitOfWork.Instructors.GetByIdAsync(request.Id, cancellationToken);

        if (instructor == null)
        {
            throw new NotFoundException($"Instructor with ID {request.Id} not found.");
        }        instructor.Name = request.InstructorRequest.Name;
        instructor.Email = request.InstructorRequest.Email;

        await _unitOfWork.Instructors.UpdateAsync(instructor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return instructor.ToInstructorResponseDTO();
    }
}
