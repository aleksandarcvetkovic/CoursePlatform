namespace CoursePlatform.Models;

public class Course
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public string InstructorId { get; set; }
    public Instructor Instructor { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }
}