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

        // GET: api/Instructor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstructorDTO>>> GetInstructorDTO()
        {
            return await _context.InstructorDTO.ToListAsync();
        }

        // GET: api/Instructor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorDTO>> GetInstructorDTO(int id)
        {
            var instructorDTO = await _context.InstructorDTO.FindAsync(id);

            if (instructorDTO == null)
            {
                return NotFound();
            }

            return instructorDTO;
        }

        // PUT: api/Instructor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstructorDTO(int id, InstructorDTO instructorDTO)
        {
            if (id != instructorDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(instructorDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorDTOExists(id))
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

        // POST: api/Instructor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InstructorDTO>> PostInstructorDTO(InstructorDTO instructorDTO)
        {
            _context.InstructorDTO.Add(instructorDTO);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInstructorDTO", new { id = instructorDTO.Id }, instructorDTO);
        }

        // DELETE: api/Instructor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructorDTO(int id)
        {
            var instructorDTO = await _context.InstructorDTO.FindAsync(id);
            if (instructorDTO == null)
            {
                return NotFound();
            }

            _context.InstructorDTO.Remove(instructorDTO);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InstructorDTOExists(int id)
        {
            return _context.InstructorDTO.Any(e => e.Id == id);
        }
    }
}
