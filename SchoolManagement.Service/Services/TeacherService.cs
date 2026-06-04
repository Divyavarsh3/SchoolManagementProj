using SchoolManagement.Model.DTOs;
using SchoolManagement.Model.Entities;
using SchoolManagement.Service.Interfaces;
using SchoolManagement.Store.Interfaces;

namespace SchoolManagement.Service.Services
{
    /// <summary>
    /// Contains Teacher Business Logic.
    /// </summary>
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        /// <summary>
        /// Create a new teacher.
        /// </summary>
        public async Task<int> CreateAsync(TeacherCreateDto teacher)
        {
            return await _teacherRepository.CreateAsync(teacher);
        }

        /// <summary>
        /// Get all teachers.
        /// </summary>
        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _teacherRepository.GetAllAsync();
        }

        /// <summary>
        /// Get teacher details by Guid.
        /// </summary>
        public async Task<Teacher?> GetByGuidAsync(Guid teacherGuid)
        {
            return await _teacherRepository.GetByGuidAsync(teacherGuid);
        }

        /// <summary>
        /// Update teacher details.
        /// </summary>
        public async Task<int> UpdateAsync(TeacherUpdateDto teacher)
        {
            return await _teacherRepository.UpdateAsync(teacher);
        }

        /// <summary>
        /// Delete teacher by Guid.
        /// </summary>
        public async Task<int> DeleteAsync(Guid teacherGuid, int updatedBy)
        {
            return await _teacherRepository.DeleteAsync(teacherGuid, updatedBy);
        }
    }
}