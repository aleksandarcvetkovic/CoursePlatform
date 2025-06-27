using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Features.Instructors.Queries;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Queries;

public class GetInstructorWithCoursesQueryHandler : IRequestHandler<GetInstructorWithCoursesQuery, InstructorWithCoursesDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInstructorWithCoursesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<InstructorWithCoursesDTO> Handle(GetInstructorWithCoursesQuery request, CancellationToken cancellationToken)
    {
        var instructor = await _unitOfWork.Instructors.GetByIdWithCoursesAsync(request.Id, cancellationToken);

        if (instructor == null)
        {
            throw new NotFoundException($"Instructor with ID {request.Id} not found.");
        }

        return instructor.ToInstructorWithCoursesDTO();
    }
}
