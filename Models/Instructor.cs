namespace CoursePlatform.Models;

public class Instructor
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public ICollection<Course> Courses { get; set; }
}