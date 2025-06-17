using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using System.Diagnostics.CodeAnalysis;

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
        [ProducesResponseType(typeof(IEnumerable<EnrollmentResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDTO>>> GetEnrollmentDTO()
        {
            var enrollments = await _context.Enrollments.Select(e => e.ToEnrolmentResponseDTO()).ToListAsync();
            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EnrollmentResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EnrollmentResponseDTO>> GetEnrollment(string id)
        {

            var enrollment = _context.Enrollments.Select(e => e.ToEnrolmentResponseDTO()).FirstOrDefault(e => e.Id == id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;

        }

        [HttpGet("EnrollmnetStudentCourse/{id}")]
        [ProducesResponseType(typeof(EnrollmentWithStudentCourseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EnrollmentWithStudentCourseDTO>> GetEnrollmentWithStudentCourseDTO(string id)
        {

            var enrollment = _context.Enrollments.Include(e => e.Course).Include(e => e.Student).Select(e => e.ToEnrollmentWithStudentCourseDTO()).FirstOrDefault(e => e.Id == id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;

        }

        [HttpPut("{id}/{grade}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutEnrollment(string id, int grade)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }


            enrollment.UpdateFromDTO(new EnrollmentGradeRequestDTO
            {
                Id = id,
                Grade = grade
            });


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
        [ProducesResponseType(typeof(EnrollmentResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Enrollment>> PostEnrollment(EnrollmentRequestDTO enrollmentRequestDTO)
        {

            var newEnrollment = enrollmentRequestDTO.ToEnrolment();
            newEnrollment.EnrolledOn = DateTime.Now;

            _context.Enrollments.Add(newEnrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnrollment", new { id = newEnrollment.Id }, newEnrollment.ToEnrolmentResponseDTO());

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
