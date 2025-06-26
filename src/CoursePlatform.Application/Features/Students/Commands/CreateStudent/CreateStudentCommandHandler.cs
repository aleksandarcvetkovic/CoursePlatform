using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Services;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace CoursePlatform.Application.Features.Students.Commands.CreateStudent;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudentValidationService _validationService;

    public CreateStudentCommandHandler(IUnitOfWork unitOfWork, IStudentValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
    }

    public async Task<StudentResponseDTO> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        // Validate student with external service
        var validationResult = await _validationService.ValidateStudentAsync(request.Student, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            var failures = validationResult.ValidationErrors
                .Select(error => new FluentValidation.Results.ValidationFailure("Student", error))
                .ToList();

                Console.WriteLine($"Service said: validation failed for student {request.Student.Email}: {string.Join(", ", validationResult.ValidationErrors)}");
            
            throw new ValidationException(failures);
        }

        var student = Student.Create(request.Student.Name, request.Student.Email);

        await _unitOfWork.Students.AddAsync(student, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return student.ToStudentResponseDTO();
    }
}
