namespace CoursePlatform.Application.DTOs;

public class InstructorRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class InstructorResponseDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class InstructorWithCoursesDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<CourseResponseDTO> Courses { get; set; } = new List<CourseResponseDTO>();
}
