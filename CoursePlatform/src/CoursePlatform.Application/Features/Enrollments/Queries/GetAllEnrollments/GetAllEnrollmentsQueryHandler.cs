using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Enrollments.Queries.GetAllEnrollments;

public class GetAllEnrollmentsQueryHandler : IRequestHandler<GetAllEnrollmentsQuery, IEnumerable<EnrollmentResponseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllEnrollmentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EnrollmentResponseDTO>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var enrollments = await _unitOfWork.Enrollments.GetAllAsync(cancellationToken);
        return enrollments.ToEnrollmentResponseDTOs();
    }
}
