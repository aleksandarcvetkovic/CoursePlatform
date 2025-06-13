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
        return Ok("ispravno");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(string id)
    {
        return Ok("working got= " + id);
    }

    [HttpPost]
    public async Task<ActionResult<Course>> PostCourse(Course course)
    {
        return Ok(course);
    }

    [HttpPut]
    public async Task<IActionResult> PutCourse(string id, Course course)
    {
        if (id != course.Id)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourseDTO(int id)
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
