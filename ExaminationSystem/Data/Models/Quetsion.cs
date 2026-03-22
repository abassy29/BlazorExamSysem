namespace ExaminationSystem.Data.Models
{
    public class Quetsion
    {
        public Guid Id { get; set; }
        public string Quet { get; set; } = string.Empty;
        // Use a List so UI can bind to indexed answers (A-D)
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public AnsLettersEnum ModelAns { get; set; } 
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }
    }
}
