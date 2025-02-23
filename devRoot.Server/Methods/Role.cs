using devRoot.Server.Models;

namespace devRoot.Server
{
    public partial class Utilities
    {
        public List<Role.RoleType> GetUserRoleTypes(string uid)
        {
            return _context.Roles.FirstOrDefault(r => r.UserUid == uid)?.Types ?? new List<Role.RoleType>();
        }
    }
}
