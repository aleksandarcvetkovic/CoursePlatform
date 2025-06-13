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
    public class InstructorController : ControllerBase
    {
        private readonly CoursePlatformContext _context;

        public InstructorController(CoursePlatformContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instructor>>> GetInstructor()
        {
            return await _context.Instructors.ToListAsync();
            

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Instructor>> GetInstructor(string id)
        {
            
            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
            {
                return NotFound();
            }

            return instructor;
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstructorDTO(InstructorDTO instructorDTO)
        {
            
            var instructor = await _context.Instructors.FindAsync(instructorDTO.Id);

            if (instructor == null)
            {
                return NotFound();
            }

            instructor.Email = instructorDTO.Email;
            instructor.Name = instructor.Name;
            instructor.Id = instructor.Id;

            _context.Entry(instructor).State = EntityState.Modified;

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
        public async Task<ActionResult<InstructorDTO>> PostInstructor(InstructorDTO instructorDTO)
        {

            Instructor instructor = new();
            instructor.Email = instructorDTO.Email;
            instructor.Name = instructorDTO.Name;
            instructor.Id = instructorDTO.Id;

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInstructor", new { id = instructorDTO.Id }, instructorDTO);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor(string id)
        {
            
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return NoContent();
            
        }

        private bool InstructorDTOExists(string id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        
        }
    }
}
