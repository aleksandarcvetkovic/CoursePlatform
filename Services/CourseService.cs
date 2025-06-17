using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;

public class CourseService : ICourseService
{
    private readonly CoursePlatformContext _context;

    public CourseService(CoursePlatformContext context)
    {
        _context = context;
    }

    public bool CanConnectToDatabase() => _context.Database.CanConnect();

    public async Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync()
    {
        return await _context.Courses.Include(c => c.Instructor)
            .Select(c => c.ToResponseDTO())
            .ToListAsync();
    }

    public async Task<CourseResponseDTO> GetCourseAsync(string id)
    {
        var course = await _context.Courses
            .Select(c => c.ToResponseDTO())
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
            throw new NotFoundException("Course not found");

        return course;
    }

    public async Task<CourseWithInstructorDTO> GetCourseWithInstructorAsync(string id)
    {
        var course = await _context.Courses.Include(c => c.Instructor)
            .Select(c => c.ToCourseWithInstructor())
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
            throw new NotFoundException("Course not found");

        return course;
    }

    public async Task<CourseResponseDTO> CreateCourseAsync(CourseRequestDTO courseDTO)
    {
        var instructor = await _context.Instructors.FindAsync(courseDTO.InstructorId);
        if (instructor == null)
            throw new BadRequestException("Instructor does not exist");

        var course = courseDTO.ToCourse();
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course.ToResponseDTO();
    }

    public async Task UpdateCourseAsync(string id, CourseRequestDTO courseDTO)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            throw new NotFoundException("Course not found");

        course.UpdateFromDTO(courseDTO);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCourseAsync(string id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            throw new NotFoundException("Course not found");

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
    }
}