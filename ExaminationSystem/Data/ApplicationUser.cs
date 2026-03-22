using ExaminationSystem.Data.Models;
using Microsoft.AspNetCore.Identity;


namespace ExaminationSystem.Data
{

    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FName { get; set; }= string.Empty;
        public string? LName { get; set; }=string.Empty;

        //Admin
        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();

        //student
        public ICollection<Course> StdCourses { get; set; } = new HashSet<Course>();
        public ICollection<StdExam> StdExams { get; set; } = new HashSet<StdExam>();

    }

}
