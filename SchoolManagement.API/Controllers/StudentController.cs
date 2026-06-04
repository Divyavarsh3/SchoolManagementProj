using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Model.DTOs;
using SchoolManagement.Service.Interfaces;

namespace SchoolManagement.API.Controllers
{
    /// <summary>
    /// Handles Student CRUD Operations, Pagination and Filtering.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Get all students with pagination and filtering.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Number of records per page.</param>
        /// <param name="studentName">Filter by student name.</param>
        /// <param name="gender">Filter by gender.</param>
        /// <param name="classId">Filter by class.</param>
        /// <returns>List of students.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string? studentName = null,
            [FromQuery] string? gender = null,
            [FromQuery] int? classId = null)
        {
            var result = await _studentService.GetAllAsync(
                pageNumber,
                pageSize,
                studentName,
                gender,
                classId);

            return Ok(result);
        }

        /// <summary>
        /// Get student details by Guid.
        /// </summary>
        /// <param name="studentGuid">Student Guid.</param>
        /// <returns>Student details.</returns>
        [HttpGet("{studentGuid}")]
        public async Task<IActionResult> GetByGuid(Guid studentGuid)
        {
            var result = await _studentService.GetByGuidAsync(studentGuid);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Create a new student.
        /// </summary>
        /// <param name="dto">Student information.</param>
        /// <returns>Created student.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(StudentCreateDto dto)
        {
            var result = await _studentService.CreateAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Update existing student details.
        /// </summary>
        /// <param name="dto">Updated student information.</param>
        /// <returns>Updated student.</returns>
        [HttpPut]
        public async Task<IActionResult> Update(StudentUpdateDto dto)
        {
            var result = await _studentService.UpdateAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Delete student by Guid.
        /// </summary>
        /// <param name="studentGuid">Student Guid.</param>
        /// <returns>Delete status.</returns>
        [HttpDelete("{studentGuid}")]
        public async Task<IActionResult> Delete(Guid studentGuid)
        {
            var result = await _studentService.DeleteAsync(studentGuid, 1);
            return Ok(result);
        }
    }
}