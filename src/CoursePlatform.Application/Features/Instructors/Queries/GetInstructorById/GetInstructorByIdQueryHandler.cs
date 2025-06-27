using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Queries.GetInstructorById;

public class GetInstructorByIdQueryHandler : IRequestHandler<GetInstructorByIdQuery, InstructorResponseDTO?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInstructorByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InstructorResponseDTO?> Handle(GetInstructorByIdQuery request, CancellationToken cancellationToken)
    {
        var instructor = await _unitOfWork.Instructors.GetByIdAsync(request.Id, cancellationToken);

        if (instructor == null)
            throw new NotFoundException($"Instructor with ID '{request.Id}' was not found.");

        return instructor.ToInstructorResponseDTO();
    }
}
