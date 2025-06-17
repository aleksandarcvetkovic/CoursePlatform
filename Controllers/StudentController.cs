using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using Namotion.Reflection;
using CoursePlatform.Services;

namespace CoursePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudentResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public async Task<ActionResult<IEnumerable<StudentResponseDTO>>> GetStudentDTO()
        {
            var studentsDTO = await _studentService.GetAllStudentsAsync();
            return Ok(studentsDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentResponseDTO>> GetStudentDTO(string id)
        {

            var student = await _studentService.GetStudentByIdAsync(id);
            return student;
        }

        [HttpGet("StudentWithEnrollments/{id}")]
        [ProducesResponseType(typeof(StudentWithEnrolmentsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentWithEnrolmentsDTO>> GetStudentWithEnrollmentsDTO(string id)
        {

            var student = await _studentService.GetStudentWithEnrollmentsAsync(id);
            return student;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutStudentDTO(string id, StudentRequestDTO studentDTO)
        {

            await _studentService.UpdateStudentAsync(id, studentDTO);
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(typeof(StudentResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentResponseDTO>> PostStudentDTO(StudentResponseDTO studentDTO)
        {
            var created = await _studentService.CreateStudentAsync(studentDTO);
            return CreatedAtAction("GetStudentDTO", new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStudentDTO(string id)
        {

            await _studentService.DeleteStudentAsync(id);
            return NoContent();
        }
    }
}
