using ExaminationSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Data.Configration
{
    public class QuetsionConfig : IEntityTypeConfiguration<Quetsion>
    {
        public void Configure(EntityTypeBuilder<Quetsion> builder)
        {
            builder.HasOne(q => q.Exam)
                .WithMany(e => e.Quetsions)
                .HasForeignKey(q => q.ExamId)
                ;
            builder.HasKey(q => q.Id);
        }
    }
}
