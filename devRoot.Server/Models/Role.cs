namespace devRoot.Server.Models
{
    public class Role
    {
        public enum RoleType
        {
            SuperUser,
            Student,
            Teacher
        }
        public RoleType Type { get; set; }
        public string UserUid { get; set; }
    }
}
