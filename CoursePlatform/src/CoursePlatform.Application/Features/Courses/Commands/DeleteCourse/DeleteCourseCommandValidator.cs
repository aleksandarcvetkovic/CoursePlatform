using FluentValidation;

namespace CoursePlatform.Application.Features.Courses.Commands;

public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
{
    public DeleteCourseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Course ID is required.")
            .Must(BeValidGuid).WithMessage("Course ID must be a valid GUID.");
    }

    private static bool BeValidGuid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}
