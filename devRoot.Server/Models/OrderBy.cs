using System.Diagnostics.Tracing;

namespace devRoot.Server.Models
{
    public enum OrderBy
    {
        None = 0,
        Title = 1,
        Tags = 2,
        Difficulty = 3,
        CreationDate = 4
    }

    public enum OrderDirection
    {
        Ascending = 0,
        Descending = 1
    }
}
