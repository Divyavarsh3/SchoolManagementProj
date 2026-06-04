using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.DTOs
{
    public class TeacherCreateDto
    {
        [Required]
        public int SubjectId { get; set; }

        [Required]
        public string TeacherName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}