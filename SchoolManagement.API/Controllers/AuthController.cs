using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Model.DTOs;
using SchoolManagement.Service.Interfaces;

namespace SchoolManagement.API.Controllers
{
    /// <summary>
    /// Handles User Authentication and JWT Token Generation.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticate user and generate JWT token.
        /// </summary>
        /// <param name="loginDto">Username and password.</param>
        /// <returns>JWT Token.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if (result == null)
            {
                return Unauthorized("Invalid Username or Password");
            }

            return Ok(result);
        }
    }
}