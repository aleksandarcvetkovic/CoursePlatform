using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;


namespace CoursePlatform.Controllers;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly CoursePlatformContext _context;

    public CourseController(CoursePlatformContext context)
    {
        this._context = context;
    }

    [HttpGet("check-db")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CheckDatabase()
    {
        try
        {
            var canConnect = _context.Database.CanConnect();
            return Ok(new { connected = canConnect });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { connected = false, error = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseWithInstructorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
    {
        var coursesDTOs = await _context.Courses.Include(c => c.Instructor).Select(c => c.ToResponseDTO()).ToListAsync();
        return Ok(coursesDTOs);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CourseResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseResponseDTO>> GetCourse(string id)
    {
        var courseDTO = _context.Courses.Select(c => c.ToResponseDTO()).FirstOrDefault(c => c.Id == id);

        if (courseDTO == null)
        {
            return NotFound();
        }

        return courseDTO;
    }

    [HttpGet("withInstructor/{id}")]
    [ProducesResponseType(typeof(CourseWithInstructorDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseWithInstructorDTO>> GetCourseWithInstructor(string id)
    {
        var course = _context.Courses.Include(c => c.Instructor).Select(c => c.ToCourseWithInstructor()).FirstOrDefault(c => c.Id == id);

        if (course == null)
        {
            return NotFound();
        }

        return course;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CourseResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Course>> PostCourse(CourseRequestDTO courseDTO)
    {

        var instructor = await _context.Instructors.FindAsync(courseDTO.InstructorId);

        if (instructor == null)
            return BadRequest("Instruktor sa datim ID ne postoji");

        var course = courseDTO.ToCourse();

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCourse", new { id = course.Id }, course.ToResponseDTO());


    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutCourse(string id, CourseRequestDTO courseDTO)
    {
        var courseEntry = await _context.Courses.FindAsync(id);

        if (courseEntry == null)
        {
            return NotFound();
        }

        courseEntry.UpdateFromDTO(courseDTO);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> DeleteCourseDTO(string id)
    {
        var courseDTO = await _context.Courses.FindAsync(id);
        if (courseDTO == null)
        {
            return NotFound();
        }

        _context.Courses.Remove(courseDTO);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}
