using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.DTOs
{
    public class StudentUpdateDto
    {
        [Required]
        public Guid StudentGuid { get; set; }

        [Required]
        [StringLength(100)]
        public string StudentName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}