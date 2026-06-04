using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.DTOs
{
    public class StudentCreateDto
    {
        [Required]
        public int ClassId { get; set; }

        [Required]
        [StringLength(100)]
        public string StudentName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}