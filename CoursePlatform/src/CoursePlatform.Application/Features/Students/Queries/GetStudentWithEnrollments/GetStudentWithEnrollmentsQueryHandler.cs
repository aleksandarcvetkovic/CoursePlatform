using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Domain.Repositories;
using MediatR;

namespace CoursePlatform.Application.Features.Students.Queries.GetStudentWithEnrollments;

public class GetStudentWithEnrollmentsQueryHandler : IRequestHandler<GetStudentWithEnrollmentsQuery, StudentWithEnrollmentsDTO?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentWithEnrollmentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentWithEnrollmentsDTO?> Handle(GetStudentWithEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.Students.GetByIdWithEnrollmentsAsync(request.Id, cancellationToken);

        if (student == null)
            throw new NotFoundException($"Student with ID '{request.Id}' was not found.");

        return student.ToStudentWithEnrollmentsDTO();
    }
}
