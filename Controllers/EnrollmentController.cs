using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using CoursePlatform.Services;

namespace CoursePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _service;

        public EnrollmentController(IEnrollmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EnrollmentResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDTO>>> GetEnrollmentDTO()
        {
            var enrollments = await _service.GetAllAsync();
            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EnrollmentResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnrollmentResponseDTO>> GetEnrollment(string id)
        {
            var enrollment = await _service.GetByIdAsync(id);
            return Ok(enrollment);
        }

        [HttpGet("EnrollmentStudentCourse/{id}")]
        [ProducesResponseType(typeof(EnrollmentWithStudentCourseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnrollmentWithStudentCourseDTO>> GetEnrollmentWithStudentCourseDTO(string id)
        {
            var enrollment = await _service.GetWithStudentCourseAsync(id);
            return Ok(enrollment);
        }

        [HttpPut("{id}/{grade}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutEnrollment(string id, int grade)
        {
            await _service.UpdateGradeAsync(id, grade);
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(typeof(EnrollmentResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EnrollmentResponseDTO>> PostEnrollment(EnrollmentRequestDTO enrollmentRequestDTO)
        {
            var created = await _service.CreateAsync(enrollmentRequestDTO);
            return CreatedAtAction(nameof(GetEnrollment), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEnrollmentDTO(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
