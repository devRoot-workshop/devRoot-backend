namespace devRoot.Server.Models
{
    public class ExampleCode : ExampleCodeRequest
    {
        public int? Id { get; set; }
    }

    public class ExampleCodeRequest
    {
        public QuestLanguage Language { get; set; }
        public string? Code { get; set; }
    }
}
