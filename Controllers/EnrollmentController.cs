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
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollmentDTO()
        {
            return await _context.Enrollments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> GetEnrollment(string id)
        {
            
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;
           
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnrollment(EnrollmentDTO enrollmentDTO)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentDTO.Id);

            if (enrollment == null)
            {
                return NotFound();
            }

            enrollment.Id = enrollmentDTO.Id;
            enrollment.CourseId = enrollmentDTO.CourseId;
            enrollment.Grade = enrollmentDTO.Grade;
            enrollment.StudentId = enrollmentDTO.StudentId;
            


            _context.Entry(enrollment).State = EntityState.Modified;


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
        public async Task<ActionResult<Enrollment>> PostEnrollment(EnrollmentDTO enrollmentDTO)
        {
            Enrollment enrollment = new();

            enrollment.Id = enrollmentDTO.Id;
            enrollment.CourseId = enrollmentDTO.CourseId;
            enrollment.Grade = enrollmentDTO.Grade;
            enrollment.StudentId = enrollmentDTO.StudentId;

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnrollment", new { id = enrollmentDTO.Id }, enrollmentDTO);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollmentDTO(string id)
        {
            
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
            

        }

        private bool EnrollmentDTOExists(string id)
        {
            return _context.Enrollments.Any(e => e.Id == id);
            
        }
    }
}
