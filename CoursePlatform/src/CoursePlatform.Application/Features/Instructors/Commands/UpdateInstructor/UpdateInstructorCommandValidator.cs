using CoursePlatform.Application.DTOs;
using FluentValidation;

namespace CoursePlatform.Application.Features.Instructors.Commands;

public class UpdateInstructorCommandValidator : AbstractValidator<UpdateInstructorCommand>
{
    public UpdateInstructorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Instructor ID is required.")
            .Must(BeValidGuid).WithMessage("Instructor ID must be a valid GUID.");

        RuleFor(x => x.InstructorRequest.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.InstructorRequest.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");
    }

    private static bool BeValidGuid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}
