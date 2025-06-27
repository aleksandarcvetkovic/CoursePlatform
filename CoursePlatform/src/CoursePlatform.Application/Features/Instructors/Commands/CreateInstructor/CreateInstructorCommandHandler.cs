using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Commands.CreateInstructor;

public class CreateInstructorCommandHandler : IRequestHandler<CreateInstructorCommand, InstructorResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateInstructorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InstructorResponseDTO> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
    {
        var instructor = request.Instructor.ToInstructor();

        await _unitOfWork.Instructors.AddAsync(instructor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return instructor.ToInstructorResponseDTO();
    }
}
