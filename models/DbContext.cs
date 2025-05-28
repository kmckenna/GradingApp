 using Microsoft.EntityFrameworkCore;

namespace GradingApp.Models
{
    public class GradingAppContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public string DbPath { get; }

        public GradingAppContext()
        {
            var path = Environment.CurrentDirectory;
            DbPath = System.IO.Path.Join(path, "grading_app.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }    
}

