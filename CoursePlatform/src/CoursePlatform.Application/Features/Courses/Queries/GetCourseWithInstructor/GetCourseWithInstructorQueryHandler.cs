using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Queries;

public class GetCourseWithInstructorQueryHandler : IRequestHandler<GetCourseWithInstructorQuery, CourseWithInstructorDTO?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCourseWithInstructorQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }    public async Task<CourseWithInstructorDTO?> Handle(GetCourseWithInstructorQuery request, CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses.GetByIdWithInstructorAsync(request.Id, cancellationToken);

        if (course == null)
            throw new NotFoundException($"Course with ID '{request.Id}' was not found.");

        return course.ToCourseWithInstructorDTO();
    }
}
