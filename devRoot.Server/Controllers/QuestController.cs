using devRoot.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace devRoot.Server.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class QuestController : Controller
{
    private readonly Utilites _utils;
    
    [HttpGet]
    [Route("getquests")]
    [FirebaseAuthorization]
    public List<Quest> GetQuests()
    {
        return _utils.GetQuests();
    }

    [HttpPost]
    [Route("createquest")]
    public IActionResult CreateQuest([FromBody] QuestRequest req)
    {
        var user = HttpContext.Items["User"];
        
        if (user != null)
        {
            _utils.RegisterQuest(req);
            return Ok();
        }
        
        return Unauthorized();
    }
}