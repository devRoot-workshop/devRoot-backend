namespace devRoot.Server.Models
{
    public class PaginatedResult<T>
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; } = new();
    }

}
