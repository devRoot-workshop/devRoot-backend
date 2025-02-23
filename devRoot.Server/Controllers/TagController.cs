using devRoot.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace devRoot.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly Utilities _utils;
        public TagController(Utilities utils)
        {
            _utils = utils;
        }

        [HttpGet]
        [Route("GetTags")]
        public List<TagDto> GetTags([FromQuery] string? searchquery = null)
        {
            return _utils.GetTags(searchquery);
        }

        [HttpGet]
        [Route("{id}/GetTag")]
        public DetailedTag? GetTag(int id)
        {
            return _utils.GetTag(id);
        }

        [HttpPut]
        [Route("{id}/Modify")]
        public IActionResult ModifyTag(int id, [FromBody] TagRequest req)
        {
            _utils.ModifyTag(id, req);
            return Ok();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterTag([FromBody] TagRequest req)
        {
            _utils.RegisterTag(req);
            return Ok();
        }
    }
}
