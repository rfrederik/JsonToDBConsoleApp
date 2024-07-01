using JsonToDBConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace JsonToDBConsoleApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=sqlite.db");
    }
}
