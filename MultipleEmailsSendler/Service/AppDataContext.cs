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
          //  Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}






