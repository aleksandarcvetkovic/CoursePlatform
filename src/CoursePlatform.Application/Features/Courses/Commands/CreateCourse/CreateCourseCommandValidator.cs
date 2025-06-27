using CoursePlatform.Application.DTOs;
using CoursePlatform.Domain.Repositories;
using FluentValidation;

namespace CoursePlatform.Application.Features.Courses.Commands;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.Course.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Course.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");    
        
        RuleFor(x => x.Course.InstructorId)
            .NotEmpty().WithMessage("InstructorId is required.")
            .Must(BeValidGuid).WithMessage("InstructorId must be a valid GUID.")
            .MustAsync(InstructorExists).WithMessage("Instructor does not exist.");
    }

    private static bool BeValidGuid(string id)
    {
        return Guid.TryParse(id, out _);
    }

    private async Task<bool> InstructorExists(string instructorId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Instructors.ExistsAsync(instructorId, cancellationToken);
    }
}
