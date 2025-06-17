namespace CoursePlatform.Models;

public class StudentWithEnrolmentsDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<EnrollmentResponseDTO>? StudentEnrollments { get; set; }

}