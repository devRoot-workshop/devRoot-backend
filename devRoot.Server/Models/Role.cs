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

    public class RoleTagCreator : TypeFilterAttribute
    {
        public RoleTagCreator() : base(typeof(RoleTagFilter))
        {
            
        }
        private class RoleTagFilter : IAuthorizationFilter
        {
            private readonly Utilites _utils;

            public RoleTagFilter(Utilites utils)
            {
                _utils = utils;
            }
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                FirebaseToken user = context.HttpContext.Items["User"] as FirebaseToken;
                try
                {
                    if (user != null)
                    {
                        if (!_utils.GetUserRoleTypes(user.Uid).Contains(Role.RoleType.TagCreator))
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }
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
