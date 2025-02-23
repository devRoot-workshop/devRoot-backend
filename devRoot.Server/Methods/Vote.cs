using devRoot.Server.Models;

namespace devRoot.Server
{
    public partial class Utilities
    {
        public void RegisterVote(Vote req)
        {
            var uservote = _context.Votes.FirstOrDefault(v => v.Uid == req.Uid && v.For == req.For && req.VoteId == v.VoteId);
            if (uservote != null)
            {
                uservote.Type = req.Type;
            }
            else
            {
                _context.Votes.Add(req);
            }
            _context.SaveChanges();
        }

        public List<VoteDto> GetUserVotes(string? uid = null, VoteFor? votefor = null, int? voteid = null)
        {
            return _context.Votes
                .Where(v => (uid == null || v.Uid == uid) &&
                           (voteid == null || v.VoteId == voteid) &&
                           (votefor == null || v.For == votefor))
                .Select(v => new VoteDto
                {
                    For = v.For,
                    Type = v.Type,
                    VoteId = v.VoteId,
                }).ToList();
        }
    }
}
