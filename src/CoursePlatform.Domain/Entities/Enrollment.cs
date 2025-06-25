using CoursePlatform.Domain.Common;

namespace CoursePlatform.Domain.Entities;

public class Enrollment : BaseEntity
{
    public string CourseId { get; set; } = string.Empty;
    public Course Course { get; set; } = null!;

    public string StudentId { get; set; } = string.Empty;
    public Student Student { get; set; } = null!;

    public DateTime EnrolledOn { get; set; }
    public double? Grade { get; set; }

    public static Enrollment Create(string studentId, string courseId)
    {
        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrolledOn = DateTime.UtcNow
        };

        return enrollment;
    }

    public void UpdateGrade(double grade)
    {
        if (grade < 0 || grade > 100)
            throw new ArgumentException("Grade must be between 0 and 100", nameof(grade));

        Grade = grade;
    }
}
