namespace devRoot.Server.Models
{
    public class Fillout : FilloutDto
    {
        public string Uid { get; set; }
    }

    public class FilloutDto
    {
        public int Id { get; set; }
        public int QuestId { get; set; }
        public DateTime CompletionTime { get; set; }
        public TimeOnly FilloutTime { get; set; }
        public string SubmittedCode { get; set; }
        public QuestLanguage SubmittedLanguage { get; set; }
    }
}
