using devRoot.Server.Models;
using Microsoft.AspNetCore.Mvc;
using devRoot.Server.Auth;
using FirebaseAdmin.Auth;

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
    public IActionResult GetQuests(
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] string? searchQuery = null,
        [FromQuery] string? sortTags = null,
        [FromQuery] QuestDifficulty sortDifficulty = QuestDifficulty.None,
        [FromQuery] QuestLanguage sortLanguage = QuestLanguage.none,
        [FromQuery] OrderBy orderBy = OrderBy.None,
        [FromQuery] OrderDirection orderDirection = OrderDirection.Ascending)
    {
        try
        {
            var firebaseToken = HttpContext.Items["User"] as FirebaseToken;
            List<int>? sortTagIds = null;
            if (!string.IsNullOrWhiteSpace(sortTags))
            {
                sortTagIds = sortTags.Split(',').Select(int.Parse).ToList();
            }
            var paginatedResult = _utils.GetQuests(pageNumber, pageSize, searchQuery, sortTagIds, sortDifficulty, sortLanguage, orderBy, orderDirection);
            return Ok(paginatedResult);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request.", details = ex.Message });
        }
    }




    [HttpPost]
    [Route("CreateQuest")]
    public IActionResult CreateQuest([FromBody] QuestRequest req)
    {
        _utils.RegisterQuest(req);
        return Ok();
    }

    [HttpGet]
    [Route("{id}/GetQuest")]
    [FirebaseAuthorization]
    public IActionResult GetQuest([FromRoute] int id)
    {
        var firebaseToken = HttpContext.Items["User"] as FirebaseToken;
        var vote = _utils.GetUserVotes(firebaseToken.Uid.ToString(), VoteFor.Quest, id).FirstOrDefault();
        VotedResult<QuestDto> result = new VotedResult<QuestDto>()
        {
            Value = _utils.GetQuest(id),
            VoteType = vote != null ? vote.Type : VoteType.None
        };

        return Ok(result); 
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