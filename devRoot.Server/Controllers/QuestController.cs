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
    public IActionResult GetQuests(
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] string? searchQuery = null,
        [FromQuery] string? sortTags = null,
        [FromQuery] QuestDifficulty sortDifficulty = QuestDifficulty.None,
        [FromQuery] QuestLanguage sortLanguage = QuestLanguage.none)
    {
        try
        {
            // Parse sortTags into a list of integers
            List<int>? sortTagIds = null;
            if (!string.IsNullOrWhiteSpace(sortTags))
            {
                sortTagIds = sortTags.Split(',')
                                     .Select(int.Parse)
                                     .ToList();
            }

            // Call the service layer to get the results
            var result = _utils.GetQuests(pageNumber, pageSize, searchQuery, sortTagIds, sortDifficulty, sortLanguage);

            var totalItems = result.Count();

            // Check if the result is null (in case of errors or no data)
            if (result == null || totalItems == 0)
            {
                return NotFound(new { message = "No quests found matching the criteria." });
            }

            var paginatedresult = new PaginatedResult<QuestDto>
            {
                Items = result,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)(pageSize ?? totalItems)),
                CurrentPage = pageNumber ?? 1,
                PageSize = pageSize ?? totalItems
            };

            // Return the paginated result
            return Ok(paginatedresult);
        }
        catch (Exception ex)
        {
            // Log the exception if necessary
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request.", details = ex.Message });
        }
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