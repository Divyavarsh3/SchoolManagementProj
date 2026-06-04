namespace SchoolManagement.Model.Entities
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        public Guid TeacherGuid { get; set; }

        public int SubjectId { get; set; }

        public string TeacherName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
}