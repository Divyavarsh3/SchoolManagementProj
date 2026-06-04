using SchoolManagement.Model.DTOs;
using SchoolManagement.Model.Entities;

namespace SchoolManagement.Store.Interfaces
{
    public interface ITeacherRepository
    {
        Task<int> CreateAsync(TeacherCreateDto teacher);

        Task<IEnumerable<Teacher>> GetAllAsync();

        Task<Teacher?> GetByGuidAsync(Guid teacherGuid);

        Task<int> UpdateAsync(TeacherUpdateDto teacher);

        Task<int> DeleteAsync(Guid teacherGuid, int updatedBy);
    }
}