using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Queries.GetAllInstructors;

public record GetAllInstructorsQuery : IRequest<IEnumerable<InstructorResponseDTO>>;
