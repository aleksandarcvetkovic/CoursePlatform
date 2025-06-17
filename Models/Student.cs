using System.ComponentModel.DataAnnotations;

namespace CoursePlatform.Models;

public class Student
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Email { get; set; }

    public ICollection<Enrollment>? Enrollments { get; set; }



}