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
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
    {
        var courses = await _context.Courses.Include(c=>c.Instructor).ToListAsync();
        var cousesDTOs = courses.ToRespondeDTOs();
        return Ok(cousesDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseResponseDTO>> GetCourse(string id)
    {
        var courseDTO = await _context.Courses.FindAsync(id);

        if (courseDTO == null)
        {
            return NotFound();
        }

        return courseDTO.ToRespondeDTO();
    }

    [HttpGet("withInstructor/{id}")]
    public async Task<ActionResult<CourseWithInstructorDTO>> GetCourseWithInstructor(string id)
    {
        var course = _context.Courses.Include(c => c.Instructor).FirstOrDefault(c => c.Id == id);
        
        if (course == null)
        {
            return NotFound();
        }
        
        return course.ToCourseWithInstructor();
    }

    [HttpPost]
    public async Task<ActionResult<Course>> PostCourse(CourseRequestDTO courseDTO)
    {

        var instructor = await _context.Instructors.FindAsync(courseDTO.InstructorId);

        if (instructor == null)
            return BadRequest("Instruktor sa datim ID ne postoji");

        var course = courseDTO.ToCourse();
        //course.Instructor = instructor;

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCourse", new { id = courseDTO.Id }, courseDTO);
        
           
    }

    [HttpPut]
    public async Task<IActionResult> PutCourse(CourseRequestDTO courseDTO)
    {
        var courseEntry = await _context.Courses.FindAsync(courseDTO.Id);

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
