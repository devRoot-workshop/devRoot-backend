namespace devRoot.Server.Models
{
    public class VotedResult<T>
    {
        public VoteType VoteType { get; set; }
        public T? Value { get; set; }
    }
}
