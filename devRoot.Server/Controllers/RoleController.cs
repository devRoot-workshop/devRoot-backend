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
        private readonly Utilities _utils;
        public RoleController(Utilities utils)
        {
            _utils = utils;
        }

        [HttpGet]
        [Route("GetUserRoleTypes")]
        [FirebaseAuthorization(AuthorizationMode.Mandatory)]
        public List<Role.RoleType> GetUserRoleTypes()
        {
            var firebaseToken = HttpContext.Items["User"] as FirebaseToken;
            if (firebaseToken is not null)
            {
                return _utils.GetUserRoleTypes(firebaseToken.Uid.ToString());
            }
            return new List<Role.RoleType>();
        }
    }
}
