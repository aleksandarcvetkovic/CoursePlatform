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
}
