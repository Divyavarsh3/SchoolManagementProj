using Microsoft.Data.SqlClient;
using SchoolManagement.Model.DTOs;
using SchoolManagement.Model.Entities;
using SchoolManagement.Store.Interfaces;
using System.Data;

namespace SchoolManagement.Store.Repositories
{
    /// <summary>
    /// Handles Student Database Operations.
    /// Executes Student Stored Procedures and Bulk Insert Operations.
    /// </summary>
    public class StudentRepository : IStudentRepository
    {
        private readonly DatabaseContext _context;

        public StudentRepository(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new student record.
        /// </summary>
        public async Task<int> CreateAsync(StudentCreateDto student)
        {
            using var connection = _context.CreateConnection();

            // Execute Student Insert Stored Procedure
            using var command = new SqlCommand("usp_Student_Insert", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ClassId", student.ClassId);
            command.Parameters.AddWithValue("@StudentName", student.StudentName);
            command.Parameters.AddWithValue("@Gender", student.Gender);
            command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
            command.Parameters.AddWithValue("@Email", student.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CreatedBy", 1);

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Retrieves students with pagination and filtering.
        /// </summary>
        public async Task<IEnumerable<Student>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? studentName,
            string? gender,
            int? classId)
        {
            var students = new List<Student>();

            using var connection = _context.CreateConnection();

            // Execute Student Pagination and Filtering Stored Procedure
            using var command = new SqlCommand("usp_Student_GetAll", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@StudentName", (object?)studentName ?? DBNull.Value);
            command.Parameters.AddWithValue("@Gender", (object?)gender ?? DBNull.Value);
            command.Parameters.AddWithValue("@ClassId", (object?)classId ?? DBNull.Value);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                students.Add(new Student
                {
                    StudentId = Convert.ToInt32(reader["StudentId"]),
                    StudentGuid = Guid.Parse(reader["StudentGuid"].ToString()!),
                    ClassId = Convert.ToInt32(reader["ClassId"]),
                    StudentName = reader["StudentName"].ToString()!,
                    Gender = reader["Gender"].ToString()!,
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                    Email = reader["Email"]?.ToString()!,
                    PhoneNumber = reader["PhoneNumber"]?.ToString()!,
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                    CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : Convert.ToInt32(reader["CreatedBy"]),
                    UpdatedOn = reader["UpdatedOn"] == DBNull.Value ? null : Convert.ToDateTime(reader["UpdatedOn"]),
                    UpdatedBy = reader["UpdatedBy"] == DBNull.Value ? null : Convert.ToInt32(reader["UpdatedBy"])
                });
            }

            return students;
        }

        /// <summary>
        /// Retrieves student details by Guid.
        /// </summary>
        public async Task<Student?> GetByGuidAsync(Guid studentGuid)
        {
            using var connection = _context.CreateConnection();

            // Execute Student Get By Guid Stored Procedure
            using var command = new SqlCommand("usp_Student_GetByGuid", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@StudentGuid", studentGuid);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Student
                {
                    StudentId = Convert.ToInt32(reader["StudentId"]),
                    StudentGuid = Guid.Parse(reader["StudentGuid"].ToString()!),
                    ClassId = Convert.ToInt32(reader["ClassId"]),
                    StudentName = reader["StudentName"].ToString()!,
                    Gender = reader["Gender"].ToString()!,
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                    Email = reader["Email"]?.ToString()!,
                    PhoneNumber = reader["PhoneNumber"]?.ToString()!,
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
        /// Updates existing student information.
        /// </summary>
        public async Task<int> UpdateAsync(StudentUpdateDto student)
        {
            using var connection = _context.CreateConnection();

            // Execute Student Update Stored Procedure
            using var command = new SqlCommand("usp_Student_Update", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@StudentGuid", student.StudentGuid);
            command.Parameters.AddWithValue("@StudentName", student.StudentName);
            command.Parameters.AddWithValue("@Email", student.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@UpdatedBy", 1);

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Deletes student by Guid.
        /// </summary>
        public async Task<int> DeleteAsync(Guid studentGuid, int updatedBy)
        {
            using var connection = _context.CreateConnection();

            // Execute Student Delete Stored Procedure
            using var command = new SqlCommand("usp_Student_Delete", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@StudentGuid", studentGuid);
            command.Parameters.AddWithValue("@UpdatedBy", updatedBy);

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Performs bulk student insertion using User Defined Table Type.
        /// </summary>
        public async Task<int> BulkInsertAsync(DataTable studentsTable, int createdBy)
        {
            using var connection = _context.CreateConnection();

            // Execute Student Bulk Insert Stored Procedure using UDT
            using var command = new SqlCommand("usp_Student_BulkInsert", connection);

            command.CommandType = CommandType.StoredProcedure;

            var tvpParam = new SqlParameter("@Students", SqlDbType.Structured)
            {
                TypeName = "dbo.Student_Type",
                Value = studentsTable
            };

            command.Parameters.Add(tvpParam);
            command.Parameters.AddWithValue("@CreatedBy", createdBy);

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }
    }
}