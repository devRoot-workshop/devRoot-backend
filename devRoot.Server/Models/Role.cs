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
}