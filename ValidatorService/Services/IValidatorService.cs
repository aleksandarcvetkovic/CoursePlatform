using FluentValidation.Results;
using ValidatorService.Models;

namespace ValidatorService.Services;

public interface IValidatorService
{
    Task<ValidationResult> ValidateStudentAsync(Student student);
}
