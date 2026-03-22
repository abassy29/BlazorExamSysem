using ExaminationSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Data.Configration
{
    public class AnswerConfig : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasOne(a=>a.Quetsion)
                .WithMany(q=>q.Answers)
                .HasForeignKey(a=>a.QuetsionId);
            ;
            builder.HasKey(a=>a.Id);
        }
    }
}
