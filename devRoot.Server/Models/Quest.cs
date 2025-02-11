namespace devRoot.Server.Models;

public class QuestDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? TaskDescription { get; set; }
    public List<ExampleCode>? ExampleCodes { get; set; }
    public string? Console { get; set; }
    public QuestDifficulty Difficulty { get; set; }
    public List<QuestLanguage>? AvailableLanguages { get; set; }
    public DateOnly Created { get; set; }
    public List<TagDto>? Tags { get; set; }
    public int Upvotes { get; set; }
    public int Downvotes { get; set; }
}


public class Quest : BaseQuest
{
    public int Id { get; set; }
    public List<Tag> Tags { get; set; }
    public DateOnly Created { get; set; }
    public List<ExampleCode>? ExampleCodes { get; set; }
}

public class BaseQuest
{
    public QuestDifficulty Difficulty { get; set; }
    public string? Title { get; set; }
    public string? TaskDescription { get; set; }
    public string? Console { get; set; }
    public List<QuestLanguage>? AvailableLanguages { get; set; }
}

public class QuestRequest : BaseQuest
{
    public List<int> TagId { get; set; } = new();
    public List<ExampleCodeRequest>? ExampleCodes { get; set; }
}

public enum QuestDifficulty
{
    None = 0,
    Easy = 1,
    Normal = 2,
    Hard = 3
}

public enum QuestLanguage
{
    none = 0,
    python = 1,
    csharp = 2,
    nextjs = 3,
    java = 4,
    cpp = 5,
    c = 6
}