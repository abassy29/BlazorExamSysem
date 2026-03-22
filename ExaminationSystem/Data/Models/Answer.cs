namespace ExaminationSystem.Data.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string Ans { get; set; }
        public AnsLettersEnum AnsLetter { get; set; }
        public Guid QuetsionId { get; set; }
        public Quetsion Quetsion { get; set; }


    }
}
