using CoursePlatform.Domain.Common;

namespace CoursePlatform.Domain.Entities;

public class Instructor : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
