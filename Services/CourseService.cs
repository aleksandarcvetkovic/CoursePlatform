using CoursePlatform.Models;
using CoursePlatform.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IInstructorRepository _instructorRepository;

    public CourseService(ICourseRepository courseRepository, IInstructorRepository instructorRepository)
    {
        _courseRepository = courseRepository;
        _instructorRepository = instructorRepository;
    }

    public async Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync()
    {
        return await _courseRepository.GetAllAsync();
    }

    public async Task<CourseResponseDTO> GetCourseAsync(string id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            throw new NotFoundException("Course not found");

        return course.ToResponseDTO();
    }

    public async Task<CourseWithInstructorDTO> GetCourseWithInstructorAsync(string id)
    {
        var courseDto = await _courseRepository.GetWithInstructorAsync(id);
        if (courseDto == null)
            throw new NotFoundException("Course not found");

        return courseDto;
    }

    public async Task<CourseResponseDTO> CreateCourseAsync(CourseRequestDTO courseDTO)
    {
        var instructor = await _instructorRepository.GetByIdAsync(courseDTO.InstructorId);
        if (instructor == null)
            throw new BadRequestException("Instructor does not exist");

        var course = courseDTO.ToCourse();
        await _courseRepository.AddAsync(course);
        return course.ToResponseDTO();
    }

    public async Task UpdateCourseAsync(string id, CourseRequestDTO courseDTO)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            throw new NotFoundException("Course not found");

        course.UpdateFromDTO(courseDTO);
        await _courseRepository.UpdateAsync(course);
    }

    public async Task DeleteCourseAsync(string id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            throw new NotFoundException("Course not found");

        await _courseRepository.DeleteAsync(course);
    }
}