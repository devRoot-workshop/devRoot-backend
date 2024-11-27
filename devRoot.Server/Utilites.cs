using System;
using System.Collections.Generic;
using System.Linq;
using devRoot.Server.Models;
using Microsoft.AspNetCore.Hosting;

namespace devRoot.Server
{
    public class Utilites
    {
        private readonly devRootContext _context;
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
                return _context.Tags.ToList();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                return null;
            }
        }

        public Tag GetTag(int id)
        {
            try
            {
                return _context.Tags.Find(id);
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
                _context.Tags.Add(new Tag {Name = request.Name });
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
