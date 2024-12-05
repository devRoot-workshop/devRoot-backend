namespace devRoot.Server.Models;

public class QuestDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string TaskDescription { get; set; }
    public DateOnly Created { get; set; }
    public List<TagDto> Tags { get; set; }
}


public class Quest : BaseQuest
{
    public int Id { get; set; }
    public List<Tag> Tags { get; set; } 
}

public class BaseQuest
{
    public string Title { get; set; }
    public string TaskDescription { get; set; }
    public DateOnly Created { get; set; }

}

public class QuestRequest : BaseQuest
{
    public List<int> TagId { get; set; } = new();
}