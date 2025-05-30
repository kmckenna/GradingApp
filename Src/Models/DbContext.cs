using Microsoft.EntityFrameworkCore;
using GradingApp.Models;

namespace GradingApp.Models
{
    public class GradingAppContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public string DbPath { get; }

        public GradingAppContext(string databasePath)
        {
            Console.WriteLine($"Database path: {databasePath}");
            DbPath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}

