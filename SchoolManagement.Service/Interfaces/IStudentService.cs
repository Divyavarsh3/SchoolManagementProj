using SchoolManagement.Model.DTOs;
using SchoolManagement.Model.Entities;
using System.Data;

namespace SchoolManagement.Service.Interfaces
{
    public interface IStudentService
    {
        Task<int> CreateAsync(StudentCreateDto student);

        Task<IEnumerable<Student>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? studentName,
            string? gender,
            int? classId);

        Task<Student?> GetByGuidAsync(Guid studentGuid);

        Task<int> UpdateAsync(StudentUpdateDto student);

        Task<int> DeleteAsync(Guid studentGuid, int updatedBy);

        Task<int> BulkInsertAsync(DataTable studentsTable, int createdBy);
    }
}