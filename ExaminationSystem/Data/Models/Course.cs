namespace ExaminationSystem.Data.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string? CourseName { get; set; }
        public Guid InstructorId { get; set; }
        public ApplicationUser? Instructor { get; set; }
        public ICollection<Exam> Exams { get; set; } = new HashSet<Exam>();

        public ICollection<ApplicationUser> Students { get; set; } = new HashSet<ApplicationUser>();

    }
}
