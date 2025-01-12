using devRoot.Server.Models;
using Microsoft.AspNetCore.Mvc;
using devRoot.Server.Auth;

namespace devRoot.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestController : Controller
{
    private readonly Utilites _utils;
    public QuestController(Utilites utils)
    {
        _utils = utils;
    }

    [HttpGet]
    [Route("GetQuests")]
    public List<QuestDto> GetQuests([FromQuery] int? PageNumber = null, [FromQuery] int? PageSize = null, [FromQuery] List<int>? SortTags = null, [FromQuery] QuestDifficulty SortDifficulty = QuestDifficulty.None, [FromQuery] QuestLanguage SortLanguage = QuestLanguage.none)
    {
        return _utils.GetQuests(PageNumber, PageSize, SortTags, SortDifficulty, SortLanguage);
    }

    [HttpGet]
    [Route("NumberOfPages")]
    [FirebaseAuthorization]
    public int NumberOfPages([FromQuery] int? PageSize = null)
    {
        int totalQuests = _utils.NumberOfQuests();
        int pageSize = PageSize.HasValue && PageSize > 0 ? PageSize.Value : 10;
        int numberOfPages = (int)Math.Ceiling((double)totalQuests / pageSize);


        return numberOfPages;
    }


    [HttpPost]
    [Route("CreateQuest")]
    [FirebaseAuthorization]
    public IActionResult CreateQuest([FromBody] QuestRequest req)
    {
        _utils.RegisterQuest(req);
        return Ok();
    }

    [HttpGet]
    [Route("{id}/GetQuest")]
    public QuestDto GetQuest([FromRoute] int id)
    {
        return _utils.GetQuest(id);
    }

    [HttpPatch]
    [Route("{id}/AddTag")]
    public IActionResult AddTag([FromRoute] int id, [FromBody] int tagid)
    {
        _utils.AddTagToQuest(id, tagid);
        return Ok();
    }

    [HttpPatch]
    [Route("{id}/RemoveTag")]
    public IActionResult RemoveTag([FromRoute] int id, [FromBody] int tagid)
    {
        _utils.RemoveTagFromQuest(id, tagid);
        return Ok();
    }
}