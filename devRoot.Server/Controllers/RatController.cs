using System;
using System.Collections.Generic;
using System.Linq;
using devRoot.Server.Auth;
using devRoot.Server.Models;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using devRoot.Server.Auth;

namespace devRoot.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RatController : Controller
    {
        [HttpGet]
        [FirebaseAuthorization]
        public IEnumerable<Rat> Get()
        {
            var user = HttpContext.Items["User"];

            if (user == null)
            {
                return null;
            }

            return Enumerable.Range(1, 5).Select(index => new Rat
            {
                Name = new List<string> { "Ratatouille", "Julius Cheeser", "Cheeseball" }[new Random().Next(3)],
                Type = (Rat.RatType)new Random().Next(3)
            })
            .ToArray();
        }

        [HttpPost]
        [Route("PostRat")]
        [Authorize(Role.RoleType.TagCreator, Role.RoleType.QuestCreator)]
        public void Post(Rat rat)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(rat));
        }

    }
}