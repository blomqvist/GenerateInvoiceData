using Microsoft.EntityFrameworkCore;
using Logic.Models;
using Common;

namespace Web.Database
{
    public class Context : DbContext
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=../db/" + Constants.DbName);
        }
    }
}
