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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
    {
        return Ok("ispravno");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(long id)
    {
        return Ok("working got= " + id);
    }

    [HttpPost]
    public async Task<ActionResult<Course>> PostCourse(Course course)
    {
        return Ok(course);
    }

    [HttpPut]
    public async Task<IActionResult> PutCourse(long id, Course course)
    {
        if (id != course.CourseId)
        {
            return BadRequest();
        }

        return NoContent();
    }
/*
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourseDTO(int id)
    {
        var courseDTO = await _context.courseDTO.FindAsync(id);
        if (instructorDTO == null)
        {
            return NotFound();
        }

        _context.InstructorDTO.Remove(instructorDTO);
        await _context.SaveChangesAsync();

        return NoContent();
    }
*/

}
