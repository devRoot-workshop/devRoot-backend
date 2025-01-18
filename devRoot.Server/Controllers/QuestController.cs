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
    public List<QuestDto> GetQuests([FromQuery] int? PageNumber = null, [FromQuery] int? PageSize = null, [FromQuery] string? SearchQuery = null, [FromQuery] string? SortTags = null, [FromQuery] QuestDifficulty SortDifficulty = QuestDifficulty.None, [FromQuery] QuestLanguage SortLanguage = QuestLanguage.none)
    {
        List<int> SortTagIds = new List<int>();
        if (!string.IsNullOrEmpty(SortTags))
        {
            foreach (string tagid in SortTags.Split(","))
            {
                SortTagIds.Add(Convert.ToInt32(tagid));
            }
        }


        return _utils.GetQuests(PageNumber, PageSize, SearchQuery, SortTagIds, SortDifficulty, SortLanguage);
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
    [Authorize(Role.RoleType.QuestCreator)]
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