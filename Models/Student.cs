using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CoursePlatform.Models;

public class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } 
    public string Name { get; set; }
    public string Email { get; set; }

    public ICollection<Enrollment>? Enrollments { get; set; }



}