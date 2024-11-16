using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace devRoot.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
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

        [HttpPost]
        [Route("PostRat")]
        public void Post(Rat rat)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(rat));
        }

    }
}
