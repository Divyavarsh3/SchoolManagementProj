using SchoolManagement.Model.DTOs;
using SchoolManagement.Model.Entities;
using SchoolManagement.Service.Interfaces;
using SchoolManagement.Store.Interfaces;
using System.Data;

namespace SchoolManagement.Service.Services
{
    /// <summary>
    /// Contains Student Business Logic.
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        /// <summary>
        /// Create a new student.
        /// </summary>
        public async Task<int> CreateAsync(StudentCreateDto student)
        {
            return await _studentRepository.CreateAsync(student);
        }

        /// <summary>
        /// Get all students with pagination and filtering.
        /// </summary>
        public async Task<IEnumerable<Student>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? studentName,
            string? gender,
            int? classId)
        {
            return await _studentRepository.GetAllAsync(
                pageNumber,
                pageSize,
                studentName,
                gender,
                classId);
        }

        /// <summary>
        /// Get student details by Guid.
        /// </summary>
        public async Task<Student?> GetByGuidAsync(Guid studentGuid)
        {
            return await _studentRepository.GetByGuidAsync(studentGuid);
        }

        /// <summary>
        /// Update student details.
        /// </summary>
        public async Task<int> UpdateAsync(StudentUpdateDto student)
        {
            return await _studentRepository.UpdateAsync(student);
        }

        /// <summary>
        /// Delete student by Guid.
        /// </summary>
        public async Task<int> DeleteAsync(Guid studentGuid, int updatedBy)
        {
            return await _studentRepository.DeleteAsync(studentGuid, updatedBy);
        }

        /// <summary>
        /// Bulk insert students using UDT.
        /// </summary>
        public async Task<int> BulkInsertAsync(DataTable studentsTable, int createdBy)
        {
            return await _studentRepository.BulkInsertAsync(studentsTable, createdBy);
        }
    }
}