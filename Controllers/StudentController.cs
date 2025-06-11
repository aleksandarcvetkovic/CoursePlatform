using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;

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

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentDTO()
        {
            return await _context.StudentDTO.ToListAsync();
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudentDTO(int id)
        {
            var studentDTO = await _context.StudentDTO.FindAsync(id);

            if (studentDTO == null)
            {
                return NotFound();
            }

            return studentDTO;
        }

        // PUT: api/Student/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentDTO(int id, StudentDTO studentDTO)
        {
            if (id != studentDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentDTOExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Student
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentDTO>> PostStudentDTO(StudentDTO studentDTO)
        {
            _context.StudentDTO.Add(studentDTO);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentDTO", new { id = studentDTO.Id }, studentDTO);
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentDTO(int id)
        {
            var studentDTO = await _context.StudentDTO.FindAsync(id);
            if (studentDTO == null)
            {
                return NotFound();
            }

            _context.StudentDTO.Remove(studentDTO);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentDTOExists(int id)
        {
            return _context.StudentDTO.Any(e => e.Id == id);
        }
    }
}
