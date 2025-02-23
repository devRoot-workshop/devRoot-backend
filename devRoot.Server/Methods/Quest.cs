using devRoot.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace devRoot.Server
{
    public partial class Utilities
    {
        public PaginatedResult<QuestDto> GetQuests(
            int? pageNumber = null,
            int? pageSize = null,
            string? searchQuery = null,
            List<int>? sortTags = null,
            QuestDifficulty difficulty = QuestDifficulty.None,
            QuestLanguage language = QuestLanguage.none,
            OrderBy orderBy = OrderBy.None,
            OrderDirection orderDirection = OrderDirection.Ascending)
        {
            try
            {
                var votes = _context.Votes;
                var query = _context.Quests.Include(q => q.Tags).Include(q => q.ExampleCodes).Select(quest =>
                    new QuestDto
                    {
                        Id = quest.Id,
                        Created = quest.Created,
                        TaskDescription = quest.TaskDescription,
                        Title = quest.Title,
                        ExampleCodes = quest.ExampleCodes,
                        Console = quest.Console,
                        Difficulty = quest.Difficulty,
                        AvailableLanguages = quest.AvailableLanguages,
                        PseudoCode = "",
                        Tags = quest.Tags.Select(t =>
                        new TagDto
                        {
                            Description = t.Description,
                            Id = t.Id,
                            Name = t.Name ?? ""
                        }).ToList(),
                        Votes = votes
                            .Where(v => v.For == VoteFor.Quest && v.VoteId == quest.Id)
                            .Sum(v => v.Type == VoteType.UpVote ? 1 : v.Type == VoteType.DownVote ? -1 : 0)
                    });

                if (sortTags != null && sortTags.Count > 0)
                {
                    List<int> sortTagSet = new List<int>(sortTags);
                    query = query.Where(q => !sortTagSet.Except(q.Tags.Select(t => t.Id)).Any());
                }
                if (difficulty != QuestDifficulty.None)
                {
                    query = query.Where(q => q.Difficulty == difficulty);
                }
                if (language != QuestLanguage.none)
                {
                    query = query.Where(q => q.AvailableLanguages.Contains(language));
                }
                if (orderBy != OrderBy.None)
                {
                    Func<IQueryable<QuestDto>, IQueryable<QuestDto>> orderFunc = orderBy switch
                    {
                        OrderBy.Title => q => orderDirection == OrderDirection.Descending ? q.OrderByDescending(quest => quest.Title) : q.OrderBy(quest => quest.Title),
                        OrderBy.Tags => q => orderDirection == OrderDirection.Descending ? q.OrderByDescending(quest => quest.Tags.Count()) : q.OrderBy(quest => quest.Tags.Count()),
                        OrderBy.Difficulty => q => orderDirection == OrderDirection.Descending ? q.OrderByDescending(quest => quest.Difficulty) : q.OrderBy(quest => quest.Difficulty),
                        OrderBy.CreationDate => q => orderDirection == OrderDirection.Descending ? q.OrderByDescending(quest => quest.Created) : q.OrderBy(quest => quest.Created),
                        _ => null
                    };
                    if (orderFunc != null)
                    {
                        query = orderFunc(query);
                    }
                }
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    query = query.Where(q => EF.Functions.Unaccent(q.Title).ToLower().Contains(EF.Functions.Unaccent(searchQuery).ToLower()) ||
                    EF.Functions.Unaccent(q.TaskDescription).ToLower().Contains(EF.Functions.Unaccent(searchQuery).ToLower()));
                }
                int totalItems = query.Count();
                List<QuestDto> items;
                if (pageNumber != null && pageSize != null && pageNumber > 0 && pageSize > 0)
                {
                    items = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
                }
                else
                {
                    items = query.ToList();
                }
                int totalPages = pageSize != null && pageSize > 0 ? (int)Math.Ceiling(totalItems / (double)pageSize.Value) : 1;
                return new PaginatedResult<QuestDto>
                {
                    Items = items,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                };
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                return new PaginatedResult<QuestDto>
                {
                    Items = new List<QuestDto>(),
                    TotalItems = 0,
                    TotalPages = 0
                };
            }
        }

        public void RegisterQuest(QuestRequest questRequest)
        {
            try
            {
                List<Tag> tags = _context.Tags.Where(tag => questRequest.TagId.Contains(tag.Id)).ToList();
                var newQuest = new Quest
                {
                    Title = questRequest.Title,
                    TaskDescription = questRequest.TaskDescription,
                    Tags = tags,
                    Difficulty = questRequest.Difficulty,
                    Created = DateOnly.FromDateTime(DateTime.Now),
                    ExampleCodes = questRequest.ExampleCodes?.Select(excode =>
                    new ExampleCode
                    {
                        Code = excode.Code,
                        Language = excode.Language
                    }).ToList(),
                    Console = questRequest.Console,
                    PseudoCode = questRequest.PseudoCode,
                    AvailableLanguages = questRequest.AvailableLanguages,
                };
                _context.Quests.Add(newQuest);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
        }

        public void AddTagToQuest(int questid, int tagid)
        {
            try
            {
                Quest refrence = _context.Quests.Include(t => t.Tags).First(q => q.Id == questid);
                Tag? tag = _context.Tags.Find(tagid);
                if (tag is not null)
                {
                    refrence.Tags.Add(tag);
                }
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
        }

        public void RemoveTagFromQuest(int questid, int tagid)
        {
            try
            {
                Quest refrence = _context.Quests.Include(t => t.Tags).First(q => q.Id == questid);
                Tag? tag = _context.Tags.Find(tagid);
                if (tag is not null)
                {
                    if (refrence.Tags.Contains(tag))
                    {
                        refrence.Tags.Remove(tag);
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
        }

        public QuestDto GetQuest(int id)
        {
            try
            {
                var votes = _context.Votes;
                return _context.Quests.Include(q => q.Tags).Select(quest =>
                new QuestDto
                {
                    Id = quest.Id,
                    Title = quest.Title,
                    Created = quest.Created,
                    TaskDescription = quest.TaskDescription,
                    ExampleCodes = quest.ExampleCodes,
                    Console = quest.Console,
                    PseudoCode = quest.PseudoCode,
                    Difficulty = quest.Difficulty,
                    AvailableLanguages = quest.AvailableLanguages,
                    Tags = quest.Tags.Select(tag =>
                    new TagDto
                    {
                        Id = tag.Id,
                        Description = tag.Description,
                        Name = tag.Name,
                    }).ToList(),
                    Votes = votes
                            .Where(v => v.For == VoteFor.Quest && v.VoteId == quest.Id)
                            .Sum(v => v.Type == VoteType.UpVote ? 1 : v.Type == VoteType.DownVote ? -1 : 0)

                }).First(q => q.Id == id);
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                return new();
            }
        }
    }
}
