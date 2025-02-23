using devRoot.Server.Models;

namespace devRoot.Server
{
    public partial class Utilities
    {
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
    }
}
