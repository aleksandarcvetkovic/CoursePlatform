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
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentDTO()
        {
            return await _context.Students.ToListAsync();
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentDTO(string id)
        {
            
            var studentDTO = await _context.Students.FindAsync(id);

            if (studentDTO == null)
            {
                return NotFound();
            }

            return studentDTO;
            
        }

        [HttpPut]
        public async Task<IActionResult> PutStudentDTO(StudentDTO studentDTO)
        {
            
        
            var studentEntry= await _context.Students.FindAsync(studentDTO.Id);

            if (studentEntry == null)
            {
                return NotFound();
            }

            studentEntry.Email = studentDTO.Email;
            studentEntry.Name = studentDTO.Name;

            _context.Entry(studentEntry).State = EntityState.Modified;

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
        public async Task<ActionResult<StudentDTO>> PostStudentDTO(StudentDTO studentDTO)
        {

            Student student = new();
            student.Email = studentDTO.Email;
            student.Id = studentDTO.Id;
            student.Name = studentDTO.Name;

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentDTO", new { id = studentDTO.Id }, studentDTO);
            
           
        }

        [HttpDelete("{id}")]
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
