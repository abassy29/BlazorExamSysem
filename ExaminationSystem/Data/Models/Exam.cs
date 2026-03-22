namespace ExaminationSystem.Data.Models
{
    public class Exam
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Quetsion> Quetsions { get; set; } = new HashSet<Quetsion>();
        public ICollection<StdExam> StdExams { get; set; } = new HashSet<StdExam>();



    }
}
