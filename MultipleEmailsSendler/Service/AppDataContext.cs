using Microsoft.EntityFrameworkCore;
using MultipleEmailsSendler.Models;

namespace MultipleEmailsSendler.Service
{
    public class AppDataContext : DbContext
    {
        public DbSet<Emails> Emails { get; set; }
        public DbSet<Recipients> Recipients { get; set; }

        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipients>()
                .HasOne(u => u.Email)
                .WithMany(c => c.Recipients)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}






