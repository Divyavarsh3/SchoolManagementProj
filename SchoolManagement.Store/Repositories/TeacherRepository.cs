using Microsoft.Data.SqlClient;
using SchoolManagement.Model.DTOs;
using SchoolManagement.Model.Entities;
using SchoolManagement.Store.Interfaces;

namespace SchoolManagement.Store.Repositories
{
    /// <summary>
    /// Handles Teacher Database Operations.
    /// Performs CRUD operations for Teacher Management.
    /// </summary>
    public class TeacherRepository : ITeacherRepository
    {
        private readonly DatabaseContext _context;

        public TeacherRepository(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new teacher record.
        /// </summary>
        public async Task<int> CreateAsync(TeacherCreateDto teacher)
        {
            using var connection = _context.CreateConnection();

            // Insert Teacher Record
            var query = @"
            INSERT INTO mst_Teacher
            (
                TeacherGuid,
                SubjectId,
                TeacherName,
                Email,
                PhoneNumber,
                IsActive,
                CreatedOn,
                CreatedBy
            )
            VALUES
            (
                @TeacherGuid,
                @SubjectId,
                @TeacherName,
                @Email,
                @PhoneNumber,
                1,
                GETDATE(),
                1
            )";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TeacherGuid", Guid.NewGuid());
            command.Parameters.AddWithValue("@SubjectId", teacher.SubjectId);
            command.Parameters.AddWithValue("@TeacherName", teacher.TeacherName);
            command.Parameters.AddWithValue("@Email", teacher.Email);
            command.Parameters.AddWithValue("@PhoneNumber", teacher.PhoneNumber);

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Retrieves all active teachers.
        /// </summary>
        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            var teachers = new List<Teacher>();

            using var connection = _context.CreateConnection();

            // Get All Active Teachers
            var query = @"SELECT * FROM mst_Teacher WHERE IsActive = 1";

            using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                teachers.Add(new Teacher
                {
                    TeacherId = Convert.ToInt32(reader["TeacherId"]),
                    TeacherGuid = Guid.Parse(reader["TeacherGuid"].ToString()!),
                    SubjectId = Convert.ToInt32(reader["SubjectId"]),
                    TeacherName = reader["TeacherName"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    PhoneNumber = reader["PhoneNumber"].ToString()!,
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                    CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : Convert.ToInt32(reader["CreatedBy"]),
                    UpdatedOn = reader["UpdatedOn"] == DBNull.Value ? null : Convert.ToDateTime(reader["UpdatedOn"]),
                    UpdatedBy = reader["UpdatedBy"] == DBNull.Value ? null : Convert.ToInt32(reader["UpdatedBy"])
                });
            }

            return teachers;
        }

        /// <summary>
        /// Retrieves teacher details by Guid.
        /// </summary>
        public async Task<Teacher?> GetByGuidAsync(Guid teacherGuid)
        {
            using var connection = _context.CreateConnection();

            // Get Teacher By Guid
            var query = @"
            SELECT *
            FROM mst_Teacher
            WHERE TeacherGuid = @TeacherGuid
            AND IsActive = 1";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TeacherGuid", teacherGuid);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Teacher
                {
                    TeacherId = Convert.ToInt32(reader["TeacherId"]),
                    TeacherGuid = Guid.Parse(reader["TeacherGuid"].ToString()!),
                    SubjectId = Convert.ToInt32(reader["SubjectId"]),
                    TeacherName = reader["TeacherName"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    PhoneNumber = reader["PhoneNumber"].ToString()!,
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                    CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : Convert.ToInt32(reader["CreatedBy"]),
                    UpdatedOn = reader["UpdatedOn"] == DBNull.Value ? null : Convert.ToDateTime(reader["UpdatedOn"]),
                    UpdatedBy = reader["UpdatedBy"] == DBNull.Value ? null : Convert.ToInt32(reader["UpdatedBy"])
                };
            }

            return null;
        }

        /// <summary>
        /// Updates existing teacher information.
        /// </summary>
        public async Task<int> UpdateAsync(TeacherUpdateDto teacher)
        {
            using var connection = _context.CreateConnection();

            // Update Teacher Record
            var query = @"
            UPDATE mst_Teacher
            SET
                TeacherName = @TeacherName,
                Email = @Email,
                PhoneNumber = @PhoneNumber,
                UpdatedOn = GETDATE(),
                UpdatedBy = 1
            WHERE TeacherGuid = @TeacherGuid
            AND IsActive = 1";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TeacherGuid", teacher.TeacherGuid);
            command.Parameters.AddWithValue("@TeacherName", teacher.TeacherName);
            command.Parameters.AddWithValue("@Email", teacher.Email);
            command.Parameters.AddWithValue("@PhoneNumber", teacher.PhoneNumber);

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Deletes teacher by Guid.
        /// Performs Soft Delete.
        /// </summary>
        public async Task<int> DeleteAsync(Guid teacherGuid, int updatedBy)
        {
            using var connection = _context.CreateConnection();

            // Soft Delete Teacher Record
            var query = @"
            UPDATE mst_Teacher
            SET
                IsActive = 0,
                UpdatedOn = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE TeacherGuid = @TeacherGuid";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TeacherGuid", teacherGuid);
            command.Parameters.AddWithValue("@UpdatedBy", updatedBy);

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }
    }
}