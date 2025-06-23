using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Enrollments.Commands.CreateEnrollment;

public record CreateEnrollmentCommand(EnrollmentRequestDTO Enrollment) : IRequest<EnrollmentResponseDTO>;
