using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Commands;

public record UpdateInstructorCommand(string Id, InstructorRequestDTO InstructorRequest) : IRequest<InstructorResponseDTO>;
