using BCrypt.Net;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Model.DTOs;
using SchoolManagement.Store.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagement.Store.Repositories
{
    /// <summary>
    /// Handles User Authentication and JWT Token Generation.
    /// Validates user credentials and creates JWT tokens.
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Validates user credentials and generates JWT token.
        /// </summary>
        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            using var connection = _context.CreateConnection();

            // Get User Details and Role Information
            var query = @"
                SELECT
                    U.UserId,
                    U.UserName,
                    U.PasswordHash,
                    R.RoleName
                FROM mst_User U
                INNER JOIN mst_Role R
                    ON U.RoleId = R.RoleId
                WHERE U.UserName = @UserName
                AND U.IsActive = 1";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", loginDto.UserName);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            var passwordHash = reader["PasswordHash"].ToString();

            if (string.IsNullOrEmpty(passwordHash))
                return null;

            // Verify BCrypt Hashed Password
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, passwordHash))
                return null;

            var userId = Convert.ToInt32(reader["UserId"]);
            var userName = reader["UserName"].ToString()!;
            var roleName = reader["RoleName"].ToString()!;

            var token = GenerateJwtToken(userId, userName, roleName);

            return new LoginResponseDto
            {
                Token = token
            };
        }

        /// <summary>
        /// Generates JWT token with User and Role Claims.
        /// </summary>
        private string GenerateJwtToken(int userId, string userName, string roleName)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, roleName)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}