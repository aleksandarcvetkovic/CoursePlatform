using CoursePlatform.Application.Features.Enrollments.Commands.CreateEnrollment;
using CoursePlatform.Domain.Repositories;
using FluentValidation;

namespace CoursePlatform.Application.Features.Enrollments.Commands.CreateEnrollment;

public class CreateEnrollmentCommandValidator : AbstractValidator<CreateEnrollmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateEnrollmentCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.Enrollment.StudentId)
            .NotEmpty().WithMessage("Student ID is required.")
            .MustAsync(StudentExists).WithMessage("Student does not exist.");

        RuleFor(x => x.Enrollment.CourseId)
            .NotEmpty().WithMessage("Course ID is required.")
            .MustAsync(CourseExists).WithMessage("Course does not exist.");

        RuleFor(x => x.Enrollment)
            .MustAsync(StudentNotAlreadyEnrolled).WithMessage("Student is already enrolled in this course.");
    }

    private async Task<bool> StudentExists(string studentId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Students.ExistsAsync(studentId, cancellationToken);
    }

    private async Task<bool> CourseExists(string courseId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Courses.ExistsAsync(courseId, cancellationToken);
    }

    private async Task<bool> StudentNotAlreadyEnrolled(CoursePlatform.Application.DTOs.EnrollmentRequestDTO enrollment, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.Enrollments.StudentAlreadyEnrolledAsync(enrollment.StudentId, enrollment.CourseId, cancellationToken);
    }
}
