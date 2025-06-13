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
    public class EnrollmentController : ControllerBase
    {
        private readonly CoursePlatformContext _context;

        public EnrollmentController(CoursePlatformContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetEnrollmentDTO()
        {
            return Ok();
            //return await _context.EnrollmentDTO.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentDTO>> GetEnrollmentDTO(int id)
        {
            /*
            var enrollmentDTO = await _context.EnrollmentDTO.FindAsync(id);

            if (enrollmentDTO == null)
            {
                return NotFound();
            }

            return enrollmentDTO;
            */
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnrollmentDTO(int id, EnrollmentDTO enrollmentDTO)
        {
            if (id != enrollmentDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(enrollmentDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentDTOExists(id))
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

        [HttpPost]
        public async Task<ActionResult<EnrollmentDTO>> PostEnrollmentDTO(EnrollmentDTO enrollmentDTO)
        {
            /*
            _context.EnrollmentDTO.Add(enrollmentDTO);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnrollmentDTO", new { id = enrollmentDTO.Id }, enrollmentDTO);
            */
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollmentDTO(int id)
        {
            /*
            var enrollmentDTO = await _context.EnrollmentDTO.FindAsync(id);
            if (enrollmentDTO == null)
            {
                return NotFound();
            }

            _context.EnrollmentDTO.Remove(enrollmentDTO);
            await _context.SaveChangesAsync();

            return NoContent();
            */
            return Ok();

        }

        private bool EnrollmentDTOExists(int id)
        {
            //return _context.EnrollmentDTO.Any(e => e.Id == id);
            return true;
        }
    }
}
