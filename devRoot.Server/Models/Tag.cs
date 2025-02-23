namespace devRoot.Server.Models
{
    public class TagDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class DetailedTag
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        required public List<int> QuestId { get; set; }
    }

    public class Tag : TagRequest
    {
        public int Id { get; set; }
        public List<Quest>? Quests { get; set; }
    }

    public class TagRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
