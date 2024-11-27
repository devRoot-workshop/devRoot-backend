namespace devRoot.Server.Models;

public class Quest : QuestRequest
{
    public string Id { get; set; }
}

public class QuestRequest
{
    public string Title { get; set; }
    public string TaskDescription { get; set; }
    public DateOnly Created { get; set; }
    public List<Tag> Tags { get; set; }
}