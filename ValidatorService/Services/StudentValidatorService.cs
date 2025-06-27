using FluentValidation;
using FluentValidation.Results;
using ValidatorService.Models;

namespace ValidatorService.Services;

public class StudentValidatorService : IValidatorService
{
    private readonly IValidator<Student> _validator;

    public StudentValidatorService(IValidator<Student> validator)
    {
        _validator = validator;
    }

    public async Task<ValidationResult> ValidateStudentAsync(Student student)
    {
        return await _validator.ValidateAsync(student);
    }
}
