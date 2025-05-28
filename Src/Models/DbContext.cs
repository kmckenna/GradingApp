using Microsoft.EntityFrameworkCore;
using GradingApp.Models;
// using GradingApp.Models.Grade;
// using Grade = GradingApp.Models.Grade;

namespace GradingApp.Models
{
    public class GradingAppContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public string DbPath { get; }

        public GradingAppContext()
        {
            var currentDirectory = Environment.CurrentDirectory;
            var path = $"{currentDirectory}/Data";
            DbPath = System.IO.Path.Join(path, "grading_app.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }    
}

