using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Queries;

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, IEnumerable<CourseResponseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCoursesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CourseResponseDTO>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _unitOfWork.Courses.GetAllAsync(cancellationToken);
        return courses.ToCourseResponseDTOs();
    }
}
