using devRoot.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace devRoot.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestController : Controller
{
    private readonly Utilites _utils;
    
    [HttpGet]
    [Route("GetQuests")]
    [FirebaseAuthorization]
    public List<Quest> GetQuests()
    {
        return _utils.GetQuests();
    }

    [HttpPost]
    [Route("CreateQuest")]
    [FirebaseAuthorization]
    public IActionResult CreateQuest([FromBody] QuestRequest req)
    {
        _utils.RegisterQuest(req);
        return Ok();
    }
}