using CoursePlatform.Application.Common.Interfaces;
using CoursePlatform.Application.Common.Mappings;
using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Commands.CreateInstructor;

public class CreateInstructorCommandHandler : IRequestHandler<CreateInstructorCommand, InstructorResponseDTO>
{
    private readonly IApplicationDbContext _context;

    public CreateInstructorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InstructorResponseDTO> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
    {
        var instructor = request.Instructor.ToInstructor();

        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync(cancellationToken);

        return instructor.ToInstructorResponseDTO();
    }
}
