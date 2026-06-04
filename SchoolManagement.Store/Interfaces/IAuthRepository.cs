using SchoolManagement.Model.DTOs;

namespace SchoolManagement.Store.Interfaces
{
    public interface IAuthRepository
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    }
}