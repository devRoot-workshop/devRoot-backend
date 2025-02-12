namespace devRoot.Server.Models
{
    public enum VoteType
    {
        UpVote,
        DownVote,
    }

    public enum VoteFor
    {
        Quest
    }
    
    public class Vote : VoteReq
    {
        public int? Id {  get; set; }
        public string? Uid { get; set; }
    }

    public class VoteReq
    {
        public VoteType Type { get; set; }
        public VoteFor For { get; set; }
        public int Id { get; set; }
    }
}
