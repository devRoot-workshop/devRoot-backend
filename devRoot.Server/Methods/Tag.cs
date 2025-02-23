using devRoot.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace devRoot.Server
{
    public partial class Utilities
    {
        public List<TagDto> GetTags(string? searchQuery = null)
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
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(q => EF.Functions.Unaccent(q.Name).ToLower().Contains(EF.Functions.Unaccent(searchQuery).ToLower()) ||
                    EF.Functions.Unaccent(q.Description).ToLower().Contains(EF.Functions.Unaccent(searchQuery).ToLower()));
                }
                return query.ToList();

            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
                return new();
            }
        }

        public DetailedTag? GetTag(int id)
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
                Tag? update = _context.Tags.Find(id);
                if (update is not null)
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
