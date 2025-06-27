using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Courses.Commands;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }   
    
    public async Task<CourseResponseDTO> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = request.Course.ToCourse();

        await _unitOfWork.Courses.AddAsync(course, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return course.ToCourseResponseDTO();
    }
}
