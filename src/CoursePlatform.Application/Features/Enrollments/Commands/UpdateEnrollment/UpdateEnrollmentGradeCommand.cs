using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Application.Features.Enrollments.Commands.UpdateEnrollmentGrade;

public record UpdateEnrollmentGradeCommand(string Id, EnrollmentGradeRequestDTO GradeRequest) : IRequest<EnrollmentResponseDTO>;
