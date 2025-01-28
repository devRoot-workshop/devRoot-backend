namespace devRoot.Server.Models
{
    public class Fillout : FilloutDto
    {
        public string Uid { get; set; }
    }

    public class FilloutDto : FilloutReq
    {
        public DateTime CompletionTime { get; set; }
        public int Id { get; set; }
    }

    public class FilloutReq
    {
        public int QuestId { get; set; }
        public TimeOnly FilloutTime { get; set; }
        public string SubmittedCode { get; set; }
        public QuestLanguage SubmittedLanguage { get; set; }
    }
}
