namespace CoursePlatform.Application.DTOs;

public record InstructorRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public record InstructorResponseDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public record InstructorWithCoursesDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<CourseResponseDTO> Courses { get; set; } = new List<CourseResponseDTO>();
}
