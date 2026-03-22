using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Data.Models
{
    [PrimaryKey("ExamId", "StdId")]
    public class StdExam
    {
        [ForeignKey("Exam")]
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }

        [ForeignKey("Std")]
        public Guid StdId { get; set; }
        public ApplicationUser Std { get; set; }
        public float grade { get; set; }


    }
}
