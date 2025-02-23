using devRoot.Server.Auth;
using devRoot.Server.Models;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;

namespace devRoot.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilloutController : ControllerBase
    {
        private readonly Utilities _utils;
        public FilloutController(Utilities utils)
        {
            _utils = utils;
        }

        [HttpGet]
        [Route("GetFillouts")]
        public List<FilloutDto> GetFillouts()
        {
            return _utils.GetFillouts();
        }

        [HttpGet]
        [Route("GetUserFillouts")]
        public List<FilloutDto> GetUserFillouts()
        {
            var firebaseToken = HttpContext.Items["User"] as FirebaseToken;
            return _utils.GetUserFillouts(firebaseToken?.Uid.ToString() ?? "");
        }

        [HttpPost]
        [Route("CreateFillout")]
        [FirebaseAuthorization(AuthorizationMode.Mandatory)]
        public ActionResult CreateFillout([FromBody] FilloutReq dto)
        {
            var firebaseToken = HttpContext.Items["User"] as FirebaseToken;
            Fillout fillout = new Fillout();
            fillout.FilloutTime = dto.FilloutTime;
            fillout.CompletionTime = DateTime.Now.ToUniversalTime();
            fillout.SubmittedLanguage = dto.SubmittedLanguage;
            fillout.SubmittedCode = dto.SubmittedCode;
            fillout.QuestId = dto.QuestId;
            if (firebaseToken is not null)
            {
                fillout.Uid = firebaseToken.Uid;
            }
            else
            {
                return Unauthorized();
            }

            _utils.CreateFillout(fillout);
            return Ok();
        }
    }
}
