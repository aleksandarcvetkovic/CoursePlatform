using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Commands.CreateInstructor;

public record CreateInstructorCommand(InstructorRequestDTO Instructor) : IRequest<InstructorResponseDTO>;
