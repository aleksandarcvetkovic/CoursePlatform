using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Students.Commands.CreateStudent;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentResponseDTO>
{
    private readonly IApplicationDbContext _context;

    public CreateStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentResponseDTO> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = request.Student.ToStudent();

        _context.Students.Add(student);
        await _context.SaveChangesAsync(cancellationToken);

        return student.ToStudentResponseDTO();
    }
}
