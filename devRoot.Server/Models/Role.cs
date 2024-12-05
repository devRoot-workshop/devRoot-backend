namespace devRoot.Server.Models
{
    public class Role
    {
        public enum RoleType
        {
            TagCreator,
            QuestCreator
        }
        public List<RoleType> Types { get; set; }
        public string? UserUid { get; set; }
    }
}
