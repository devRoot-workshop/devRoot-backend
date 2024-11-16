using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;

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
         * Kicsit gányolás
         * 
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
