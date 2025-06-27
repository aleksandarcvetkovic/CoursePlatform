using CoursePlatform.Application.Common.Exceptions;
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
            Console.WriteLine($"Validation failed: {validationResult.ErrorMessage}");
            throw new BadRequestException(validationResult.ErrorMessage);
        }

        var student = Student.Create(request.Student.Name, request.Student.Email);

        await _unitOfWork.Students.AddAsync(student, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return student.ToStudentResponseDTO();
    }
}
