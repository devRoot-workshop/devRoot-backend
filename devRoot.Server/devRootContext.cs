using devRoot.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace devRoot.Server
{
    public class devRootContext(DbContextOptions<devRootContext> options) : DbContext(options)
    {
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Quest>? Quests { get; set; }
        public DbSet<Role>? Roles { get; set; }
        public DbSet<Fillout> Fillouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tag>().Property(k => k.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Quest>().Property(k => k.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property(k => k.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Fillout>().Property(k => k.Id).ValueGeneratedOnAdd();

            //many to many
            modelBuilder.Entity<Quest>()
                .HasMany(q => q.Tags)
                .WithMany(q => q.Quests)
                .UsingEntity("QuestTagJoin");
        }
    }
}
