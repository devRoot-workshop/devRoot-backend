using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace devRoot.Server
{
    public class devRootContext(DbContextOptions<devRootContext> options) : DbContext(options)
    {
        public DbSet<Tag> Tags { get; set; }
    }
}
