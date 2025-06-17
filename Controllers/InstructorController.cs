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
        [ProducesResponseType(typeof(IEnumerable<InstructorResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InstructorResponseDTO>>> GetInstructor()
        {
            var instructorsDTOs = _context.Instructors.Select(i => i.ToInstructorResponseDTO()).ToList();
            return Ok(instructorsDTOs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InstructorResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorResponseDTO>> GetInstructor(string id)
        {

            var instructor = _context.Instructors.Select(i => i.ToInstructorResponseDTO()).FirstOrDefault(i => i.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return instructor;

        }
        [HttpGet("withCourses/{id}")]
        [ProducesResponseType(typeof(InstructorWithCoursesDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorWithCoursesDTO>> GetInstructorWithCourses(string id)
        {

            var instructor = _context.Instructors.Include(i => i.Courses).Select(i => i.ToInstructorWithCoursesDTO()).FirstOrDefault(i => i.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return instructor;

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutInstructorDTO(string id, InstructorRequestDTO instructorRequestDTO)
        {

            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
            {
                return NotFound();
            }

            instructor.UpdateFromDTO(instructorRequestDTO);

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
        [ProducesResponseType(typeof(InstructorResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorResponseDTO>> PostInstructor(InstructorRequestDTO instructorRequestDTO)
        {

            var instructor = instructorRequestDTO.ToInstructor();
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();


            if (instructor== null)
            {
                return BadRequest("Instructor could not be created.");
            }
            
            return CreatedAtAction("GetInstructor", new { id = instructor.Id }, instructor.ToInstructorResponseDTO());

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
