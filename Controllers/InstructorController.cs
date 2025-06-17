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
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InstructorResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InstructorResponseDTO>>> GetInstructor()
        {
            var instructorsDTOs = await _instructorService.GetAllInstructorsAsync();
            return Ok(instructorsDTOs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InstructorResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorResponseDTO>> GetInstructor(string id)
        {
            var instructor = await _instructorService.GetInstructorByIdAsync(id);

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
            var instructor = await _instructorService.GetInstructorWithCoursesAsync(id);

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
            await _instructorService.UpdateInstructorAsync(id, instructorRequestDTO);
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(typeof(InstructorResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorResponseDTO>> PostInstructor(InstructorRequestDTO instructorRequestDTO)
        {
            var created = await _instructorService.CreateInstructorAsync(instructorRequestDTO);
            return CreatedAtAction(nameof(GetInstructor), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteInstructor(string id)
        {
            await _instructorService.DeleteInstructorAsync(id);
            return NoContent();
        }
    }
}
