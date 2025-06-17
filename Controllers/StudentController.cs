using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using Namotion.Reflection;

namespace CoursePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly CoursePlatformContext _context;

        public StudentController(CoursePlatformContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentDTO()
        {
            var studentsDTO = await _context.Students.Select(s => s.ToStudentDTO()).ToListAsync();
            return Ok(studentsDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentDTO(string id)
        {

            var student = _context.Students.Select(s => s.ToStudentDTO()).FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return student;

        }


        [HttpGet("StudentWithEnrollments/{id}")]
        [ProducesResponseType(typeof(StudentWithEnrolmentsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentWithEnrolmentsDTO>> GetStudentWithEnrollmentsDTO(string id)
        {

            var student = await _context.Students.Include(s => s.Enrollments).Select(s => s.ToStudentWithEnrolmentsDTO()).FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return student;

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutStudentDTO(StudentDTO studentDTO)
        {


            var studentEntry = await _context.Students.FindAsync(studentDTO.Id);

            if (studentEntry == null)
            {
                return NotFound();
            }

            studentEntry.UpdateFromDTO(studentDTO);

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

        [HttpPost]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDTO>> PostStudentDTO(StudentDTO studentDTO)
        {

            _context.Students.Add(studentDTO.ToStudent());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentDTO", new { id = studentDTO.Id }, studentDTO);


        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStudentDTO(string id)
        {

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        private bool StudentDTOExists(string id)
        {
            return _context.Students.Any(e => e.Id == id);
            
        }
    }
}
