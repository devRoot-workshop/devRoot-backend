using devRoot.Server.Models;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel;

namespace devRoot.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilloutController : ControllerBase
    {
        private readonly Utilites _utils;
        public FilloutController(Utilites utils)
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

            return _utils.GetUserFillouts(firebaseToken.Uid);
        }

        [HttpPost]
        [Route("CreateFillout")]
        public ActionResult CreateFillout([FromBody] FilloutDto dto)
        {
            var firebaseToken = HttpContext.Items["User"] as FirebaseToken;
            Fillout fillout = new Fillout();
            fillout.FilloutTime = dto.FilloutTime;
            fillout.CompletionTime = DateTime.Now;
            fillout.SubmittedLanguage = dto.SubmittedLanguage;
            fillout.SubmittedCode = dto.SubmittedCode;
            fillout.QuestId = dto.QuestId;
            fillout.Uid = firebaseToken.Uid;

            _utils.CreateFillout(fillout);
            return Ok();
        }
    }
}
