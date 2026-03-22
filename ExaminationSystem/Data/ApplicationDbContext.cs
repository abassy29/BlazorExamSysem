using ExaminationSystem.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
        public virtual DbSet<Course> Courses => Set<Course>();
        public virtual DbSet<Exam> Exams => Set<Exam>();
        public virtual DbSet<Quetsion> Quetsions => Set<Quetsion>();
        public virtual DbSet<Answer> Answers => Set<Answer>();
        public virtual DbSet<StdExam> StdExams => Set<StdExam>();



    }
}
