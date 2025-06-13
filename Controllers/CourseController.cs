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
        return await _context.Courses.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(string id)
    {
        var courseDTO = await _context.Courses.FindAsync(id);

            if (courseDTO == null)
            {
                return NotFound();
            }

            return courseDTO;
    }

    [HttpPost]
    public async Task<ActionResult<Course>> PostCourse(CourseDTO courseDTO)
    {

            Course course = new();

            course.Id = courseDTO.Id;
            course.Title = courseDTO.Title;
            course.Description = courseDTO.Description;
            course.InstructorId = courseDTO.InstructorId;
        
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = courseDTO.Id }, courseDTO);
            
           
    }

    [HttpPut]
    public async Task<IActionResult> PutCourse(CourseDTO courseDTO)
    {
            var courseEntry = await _context.Courses.FindAsync(courseDTO.Id);

            if (courseEntry == null)
            {
                return NotFound();
            }

            courseEntry.Id = courseDTO.Id;
            courseEntry.Title = courseDTO.Title;
            courseEntry.Description = courseDTO.Description;
            courseEntry.InstructorId = courseEntry.InstructorId;
            

            _context.Entry(courseEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
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
