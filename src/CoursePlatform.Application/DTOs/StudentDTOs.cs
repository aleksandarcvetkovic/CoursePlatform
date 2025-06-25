namespace CoursePlatform.Application.DTOs;

public class StudentRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class StudentResponseDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class StudentWithEnrollmentsDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<EnrollmentResponseDTO> Enrollments { get; set; } = new List<EnrollmentResponseDTO>();
}
