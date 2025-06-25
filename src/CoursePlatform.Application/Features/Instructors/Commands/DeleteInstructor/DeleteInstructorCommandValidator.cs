using FluentValidation;

namespace CoursePlatform.Application.Features.Instructors.Commands;

public class DeleteInstructorCommandValidator : AbstractValidator<DeleteInstructorCommand>
{
    public DeleteInstructorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Instructor ID is required.")
            .Must(BeValidGuid).WithMessage("Instructor ID must be a valid GUID.");
    }

    private static bool BeValidGuid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}
