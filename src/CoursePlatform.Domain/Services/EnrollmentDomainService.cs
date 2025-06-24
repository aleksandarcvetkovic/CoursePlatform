using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;

namespace CoursePlatform.Domain.Services;

public interface IEnrollmentDomainService
{
    Task<Enrollment> EnrollStudentInCourseAsync(string studentId, string courseId, CancellationToken cancellationToken = default);
    Task ValidateEnrollmentAsync(string studentId, string courseId, CancellationToken cancellationToken = default);
}

public class EnrollmentDomainService : IEnrollmentDomainService
{
    private readonly IUnitOfWork _unitOfWork;

    public EnrollmentDomainService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Enrollment> EnrollStudentInCourseAsync(string studentId, string courseId, CancellationToken cancellationToken = default)
    {
        await ValidateEnrollmentAsync(studentId, courseId, cancellationToken);
        
        return Enrollment.Create(studentId, courseId);
    }

    public async Task ValidateEnrollmentAsync(string studentId, string courseId, CancellationToken cancellationToken = default)
    {
        // Check if student exists
        var studentExists = await _unitOfWork.Students.ExistsAsync(studentId, cancellationToken);
        if (!studentExists)
            throw new ArgumentException($"Student with ID '{studentId}' does not exist.");

        // Check if course exists
        var courseExists = await _unitOfWork.Courses.ExistsAsync(courseId, cancellationToken);
        if (!courseExists)
            throw new ArgumentException($"Course with ID '{courseId}' does not exist.");

        // Check if student is already enrolled
        var alreadyEnrolled = await _unitOfWork.Enrollments.StudentAlreadyEnrolledAsync(studentId, courseId, cancellationToken);
        if (alreadyEnrolled)
            throw new InvalidOperationException($"Student is already enrolled in course '{courseId}'.");
    }
}
