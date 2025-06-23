using CoursePlatform.Domain.Common;

namespace CoursePlatform.Domain.Entities;

public class Student : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
