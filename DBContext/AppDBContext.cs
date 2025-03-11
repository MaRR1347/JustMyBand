using Microsoft.EntityFrameworkCore;
using BlazorApp1.Models;
using System.Diagnostics;

namespace BlazorApp1.DBContext
{
    public class AppDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite(@"Data Source=Database\music.db");

        public DbSet<Songs> songs { get; set; }
        public DbSet<Queue> queue { get; set; }
    }
}
