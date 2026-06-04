using SchoolManagement.Model.DTOs;

namespace SchoolManagement.Service.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    }
}