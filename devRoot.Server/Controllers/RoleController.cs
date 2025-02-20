using devRoot.Server.Auth;
using devRoot.Server.Models;
using FirebaseAdmin.Auth;
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
        [Route("GetUserRoleTypes")]
        [FirebaseAuthorization(AuthorizationMode.Mandatory)]
        public List<Role.RoleType> GetUserRoleTypes()
        {
            var firebaseToken = HttpContext.Items["User"] as FirebaseToken;
            return _utils.GetUserRoleTypes(firebaseToken.Uid.ToString());
        }
    }
}
