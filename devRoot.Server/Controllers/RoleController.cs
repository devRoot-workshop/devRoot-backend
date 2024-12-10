using devRoot.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace devRoot.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private readonly IHostEnvironment hostEnvironment;
        
        private readonly Utilites _utils;
        public RoleController(Utilites utils)
        {
            _utils = utils;
        }
        [HttpGet]
        [Route("GetRoleTypes")]
        public List<Role.RoleType> GetRoleTypes(string uid)
        {
            return _utils.GetUserRoleTypes(uid);
        }
    }
}
