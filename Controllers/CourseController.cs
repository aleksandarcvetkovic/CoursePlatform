using Microsoft.AspNetCore.Mvc;
using CoursePlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoursePlatform.Services;
namespace CoursePlatform.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;
    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;

    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CourseResponseDTO>>> GetCourses()
    {
        var coursesDTOs = await _courseService.GetAllCoursesAsync();
        return Ok(coursesDTOs);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CourseResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseResponseDTO>> GetCourse(string id)
    {
        var courseDTO = await _courseService.GetCourseAsync(id);
        return Ok(courseDTO);
    }

    [HttpGet("withInstructor/{id}")]
    [ProducesResponseType(typeof(CourseWithInstructorDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseWithInstructorDTO>> GetCourseWithInstructor(string id)
    {
        var course = await _courseService.GetCourseWithInstructorAsync(id);
        return Ok(course);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CourseResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseResponseDTO>> PostCourse(CourseRequestDTO courseDTO)
    {
        var createdCourse = await _courseService.CreateCourseAsync(courseDTO);
        return CreatedAtAction(nameof(GetCourse), new { id = createdCourse.Id }, createdCourse);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutCourse(string id, CourseRequestDTO courseDTO)
    {
        await _courseService.UpdateCourseAsync(id, courseDTO);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCourseDTO(string id)
    {
        await _courseService.DeleteCourseAsync(id);
        return NoContent();
    }
}
