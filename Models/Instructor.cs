using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoursePlatform.Models;

public class Instructor
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Email { get; set; }

    public ICollection<Course> Courses { get; set; }
}