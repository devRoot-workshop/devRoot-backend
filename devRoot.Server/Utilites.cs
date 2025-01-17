using System;
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

        #region Quest

        public List<QuestDto> GetQuests(int? pagenumber = null, int? pagesize = null)
        {
            try
            {
                List<Quest> quests = _context.Quests.Include(q => q.Tags).ToList();
                var query = quests.Select(quest =>
                    new QuestDto
                    {
                        Id = quest.Id,
                        Created = quest.Created,
                        TaskDescription = quest.TaskDescription,
                        Title = quest.Title,
                        Code = quest.Code,
                        Console = quest.Console,
                        Difficulty = quest.Difficulty,
                        Language = quest.Language,
                        Tags = quest.Tags.Select(t => 
                        new TagDto
                        {
                            Description = t.Description,
                            Id = t.Id,
                            Name = t.Name
                        }).ToList()
                    }).ToList();

                if (pagenumber != null && pagesize != null)
                {
                    if (pagenumber > 0 && pagesize > 0)
                    {
                        int _pagenumber = pagenumber ?? default(int);
                        int _pagesize = pagesize ?? default(int);
                        return query.Skip((_pagenumber-1)*_pagesize).Take(_pagesize).ToList();
                    }
                    return null;
                }
                else
                {
                    return query;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                return null;
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
                    Code = questRequest.Code,
                    Console = questRequest.Console,
                    Language = questRequest.Language,
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
                    Code = q.Code,
                    Console = q.Console,
                    Difficulty = q.Difficulty,
                    Language = q.Language,
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
                return null;
            }
        }
        
        #endregion
        
        public List<TagDto> GetTags()
        {
            try
            {
                return _context.Tags.Select(tag =>
                new TagDto
                {
                    Id = tag.Id,
                    Description = tag.Description,
                    Name = tag.Name,
                }).ToList();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                return null;
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
                }).FirstOrDefault();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                return null;
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
    }
}
