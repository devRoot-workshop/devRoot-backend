using devRoot.Server.Auth;
using devRoot.Server.Models;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;

namespace devRoot.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoteController : Controller
    {
        private readonly Utilites _utils;
        public VoteController(Utilites utils)
        {
            _utils = utils;
        }

        [HttpPost]
        [Route("Vote")]
        [FirebaseAuthorization]
        public IActionResult Vote([FromBody] VoteReq voteReq)
        {
            try
            {
                var firebaseToken = HttpContext.Items["User"] as FirebaseToken;
                Vote vote = new Vote
                {
                    For = voteReq.For,
                    Type = voteReq.Type,
                    VoteId = voteReq.VoteId,
                    Uid = firebaseToken?.Uid
                };
                _utils.RegisterVote(vote);
            }
            catch (Exception)
            {
                
            }
            return Ok();
        }
    }
}
