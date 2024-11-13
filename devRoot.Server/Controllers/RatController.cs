using Microsoft.AspNetCore.Mvc;

namespace devRoot.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatController : Controller
    {
        [HttpGet(Name = "GetRats")]
        public IEnumerable<Rat> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Rat
            {
                Name = new List<string> { "Ratatouille", "Julius Cheeser", "Cheeseball" }[new Random().Next(3)],
                Type = (Rat.RatType)new Random().Next(3)
            })
            .ToArray();
        }

        [HttpPost(Name = "PostRat")]
        public void Post(Rat rat)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(rat));
        }

    }
}
