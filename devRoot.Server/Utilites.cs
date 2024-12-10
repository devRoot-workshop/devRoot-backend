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

        public List<Role.RoleType> GetUserRoleTypes(string uid)
        {
            var a = _context.Roles.First(r => r.UserUid == uid).Types;
            return a.ToList();
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

        public List<Quest> GetQuests()
        {
            try
            {
                return _context.Quests.ToList();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                return null;
            }
        }

        public void RegisterQuest(QuestRequest quest)
        {
            try
            {
                _context.Quests.Add(new Quest()
                {
                    Title = quest.Title,
                    TaskDescription = quest.TaskDescription,
                    Created = DateOnly.FromDateTime(DateTime.Now),
                    Tags = quest.Tags
                });
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
        }

        public List<Tag> GetTags()
        {
            try
            {
                return _context.Quests.Find(id);
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
                _context.Tags.Add(new Tag { Name = request.Name, Description = request.Description });
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
