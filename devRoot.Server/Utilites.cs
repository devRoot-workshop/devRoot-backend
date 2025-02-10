using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using devRoot.Server.Models;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace devRoot.Server
{
    public class Utilites
    {
        private readonly devRootContext? _context;
        private readonly IWebHostEnvironment _environment;
        public Utilites(devRootContext context, IWebHostEnvironment env)
        {
            _context = context;
            _environment = env;
        }

        public bool DbOk()
        {
            return _context.Database.CanConnect();
        }

        #region User
        public async Task<UserRecord> GetUserAsync(string uid)
        {
            return await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
        }
        #endregion
        #region Role
        public List<Role.RoleType> GetUserRoleTypes(string uid)
        {
            var a = _context.Roles.First(r => r.UserUid == uid).Types;
            return a.ToList();
        }
        #endregion

        /* 
        public IActionResult Return(object o, Type? t)
        {
            if (t?.IsInstanceOfType(o) == true)
            {
                return new OkObjectResult(o);
            }

            if (_environment.IsDevelopment())
            {
                return new ObjectResult(new { Error = "Invalid Type", Data = o })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        public object GetTags()
        {
            try
            {
                Return(_context.Tags.ToList(), typeof(List<Tag>));
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                Return(e, typeof(Exception));
            }
            return null;
        }
        */


        //used to ignore the difference btwn "í" and "i", "á" and "a" etc.
        private static string RemoveDiacritics(string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                    .Replace("\u0301", "")
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            ).Normalize(NormalizationForm.FormC);
        }


        #region Quest

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
                var query = _context.Quests.Include(q => q.Tags).Include(q => q.ExampleCodes).ToList().Select(quest => new QuestDto
                {
                    Id = quest.Id,
                    Created = quest.Created,
                    TaskDescription = quest.TaskDescription,
                    Title = quest.Title,
                    ExampleCodes = quest.ExampleCodes,
                    Console = quest.Console,
                    Difficulty = quest.Difficulty,
                    AvailableLanguages = quest.AvailableLanguages,
                    Tags = quest.Tags.Select(t => new TagDto
                    {
                        Description = t.Description,
                        Id = t.Id,
                        Name = t.Name ?? ""
                    }).ToList()
                });
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    var normalizedSearch = RemoveDiacritics(searchQuery);
                    query = query.Where(q => RemoveDiacritics(q.Title).Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase) ||
                                            RemoveDiacritics(q.TaskDescription).Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase));
                }
                if (sortTags != null && sortTags.Count > 0)
                {
                    var sortTagSet = new HashSet<int>(sortTags);
                    query = query.Where(q =>
                    {
                        var questTagIds = new HashSet<int>(q.Tags.Select(t => (int)t.Id));
                        return sortTagSet.IsSubsetOf(questTagIds);
                    });
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
                    Func<IEnumerable<QuestDto>, IOrderedEnumerable<QuestDto>> orderFunc = orderBy switch
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
                    ExampleCodes = questRequest?.ExampleCodes?.Select(excode =>
                    new ExampleCode {
                        Code = excode.Code,
                        Language = excode.Language }).ToList(),
                    Console = questRequest.Console,
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
                Quest refrence = _context.Quests.Include(t=>t.Tags).First(q => q.Id == questid);
                Tag tag = _context.Tags.Find(tagid);
                if (tag != null)
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
                Tag tag = _context.Tags.Find(tagid);
                if (tag != null)
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
                return _context.Quests.Include(q => q.Tags).Select(q =>
                new QuestDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Created = q.Created,
                    TaskDescription = q.TaskDescription,
                    ExampleCodes = q.ExampleCodes,
                    Console = q.Console,
                    Difficulty = q.Difficulty,
                    AvailableLanguages = q.AvailableLanguages,
                    Tags = q.Tags.Select(tag =>
                    new TagDto
                    {
                        Id = tag.Id,
                        Description = tag.Description,
                        Name = tag.Name,
                    }).ToList()

                }).First(q => q.Id == id);
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                return new();
            }
        }
        
        #endregion
        

        public int NumberOfQuests()
        {
            return _context.Quests.Count();
        }

        public List<TagDto> GetTags(string? searchquery = null)
        {
            try
            {
                var query = _context.Tags.Select(tag =>
                new TagDto
                {
                    Id = tag.Id,
                    Description = tag.Description,
                    Name = tag.Name,
                });
                if (!String.IsNullOrEmpty(searchquery))
                {
                    query = query.Where(t =>
                    RemoveDiacritics(t.Name).Contains(RemoveDiacritics(searchquery), StringComparison.OrdinalIgnoreCase)
                    || RemoveDiacritics(t.Description).Contains(RemoveDiacritics(searchquery), StringComparison.OrdinalIgnoreCase));
                }
                return query.ToList();

            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                return new();
            }
        }

        public DetailedTag GetTag(int id)
        {
            try
            {
                return _context.Tags.Include(t => t.Quests).Where(t => t.Id == id).Select(tag =>
                new DetailedTag
                {
                    Description = tag.Description,
                    Id = tag.Id,
                    Name = tag.Name,
                    QuestId = tag.Quests.Select(t => t.Id).ToList()
                }).FirstOrDefault() ?? new();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                return new();
            }
        }



        public void RegisterTag(TagRequest request)
        {
            try
            {
                _context.Tags.Add(new Tag {Name = request.Name, Description = request.Description });
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
        }

        public void ModifyTag(int id, TagRequest request)
        {
            try
            {
                Tag update = _context.Tags.Find(id);
                if (update != null)
                {
                    update.Name = request.Name;
                    _context.Update(update);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
        }

        #region Fillout

        public List<FilloutDto> GetFillouts()
        {
            return _context.Fillouts.Select(f => new FilloutDto
            {
                Id = f.Id,
                CompletionTime = f.CompletionTime,
                SubmittedCode = f.SubmittedCode,
                SubmittedLanguage = f.SubmittedLanguage,
                FilloutTime = f.FilloutTime,
                QuestId = f.QuestId,
            }).ToList();
        }

        public FilloutDto GetFillout(int id)
        {
            return _context.Fillouts.Select(f => new FilloutDto
            {
                Id = f.Id,
                CompletionTime = f.CompletionTime,
                SubmittedCode = f.SubmittedCode,
                SubmittedLanguage = f.SubmittedLanguage,
                FilloutTime = f.FilloutTime,
                QuestId = f.QuestId,
            }).First(f => f.Id == id);
        }


        public List<FilloutDto> GetUserFillouts(string uid)
        {
            return _context.Fillouts.Where(f => f.Uid == uid).Select(f => new FilloutDto
            {
                Id = f.Id,
                CompletionTime = f.CompletionTime,
                SubmittedCode = f.SubmittedCode,
                SubmittedLanguage = f.SubmittedLanguage,
                FilloutTime = f.FilloutTime,
                QuestId = f.QuestId,
            }).ToList();
        }

        public void CreateFillout(Fillout fillout)
        {
            _context.Fillouts.Add(fillout);
            _context.SaveChanges();
        }

        #endregion
    }
}
