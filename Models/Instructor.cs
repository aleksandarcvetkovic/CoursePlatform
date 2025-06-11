namespace CoursePlatform.Models;

public class Instructor
{
    public int InstructorId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public ICollection<Course> Courses { get; set; }
}