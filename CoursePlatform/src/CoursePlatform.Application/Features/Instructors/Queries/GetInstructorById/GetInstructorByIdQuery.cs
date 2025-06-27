using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Instructors.Queries.GetInstructorById;

public record GetInstructorByIdQuery(string Id) : IRequest<InstructorResponseDTO?>;
