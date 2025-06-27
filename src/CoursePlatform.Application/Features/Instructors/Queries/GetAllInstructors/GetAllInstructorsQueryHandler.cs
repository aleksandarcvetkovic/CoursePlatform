using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Queries.GetAllInstructors;

public class GetAllInstructorsQueryHandler : IRequestHandler<GetAllInstructorsQuery, IEnumerable<InstructorResponseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllInstructorsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<InstructorResponseDTO>> Handle(GetAllInstructorsQuery request, CancellationToken cancellationToken)
    {
        var instructors = await _unitOfWork.Instructors.GetAllAsync(cancellationToken);
        return instructors.ToInstructorResponseDTOs();
    }
}
