namespace CoursePlatform.Models;

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }
}