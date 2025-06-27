namespace CoursePlatform.Application.DTOs;

public record StudentRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public record StudentResponseDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public record StudentWithEnrollmentsDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<EnrollmentResponseDTO> Enrollments { get; set; } = new List<EnrollmentResponseDTO>();
}
