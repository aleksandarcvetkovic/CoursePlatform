using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Enrollments.Queries.GetAllEnrollments;

public record GetAllEnrollmentsQuery : IRequest<IEnumerable<EnrollmentResponseDTO>>;
