namespace devRoot.Server.Models
{
    public class Tag : TagRequest
    {
        public int? ID { get; set; }
    }

    public class TagRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

    }
}
