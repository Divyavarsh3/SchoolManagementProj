using SchoolManagement.Model.DTOs;
using SchoolManagement.Service.Interfaces;
using SchoolManagement.Store.Interfaces;

namespace SchoolManagement.Service.Services
{
    /// <summary>
    /// Handles Authentication Business Logic.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        /// <summary>
        /// Authenticate user and generate JWT token.
        /// </summary>
        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            return await _authRepository.LoginAsync(loginDto);
        }
    }
}