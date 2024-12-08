using System.CodeDom;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace devRoot.Server.Models
{
    public class Role
    {
        public int Id { get; set; }
        public enum RoleType
        {
            TagCreator,
            QuestCreator
        }
        public List<RoleType> Types { get; set; }
        public string? UserUid { get; set; }
    }

    public class Authorize : TypeFilterAttribute
    {
        public Authorize(params Role.RoleType[] allowedRoles)
            : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { allowedRoles };
        }

        private class AuthorizeFilter : IAuthorizationFilter
        {
            private readonly Utilites _utils;
            private readonly List<Role.RoleType> _allowedRoles;

            public AuthorizeFilter(Utilites utils, Role.RoleType[] allowedRoles)
            {
                _utils = utils;
                _allowedRoles = allowedRoles.ToList();
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                FirebaseToken user = context.HttpContext.Items["User"] as FirebaseToken;

                try
                {
                    if (user != null)
                    {
                        var userRoles = _utils.GetUserRoleTypes(user.Uid);
                        if (!_allowedRoles.Any(role => userRoles.Contains(role)))
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }
                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }
                catch
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }

}
