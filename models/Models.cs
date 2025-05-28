using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using System;
using System.Collections.Generic;

// public class GradingAppContext : DbContext
// {
//     public DbSet<Student> Students { get; set; }
//     public DbSet<Grade> Grades { get; set; }

//     public string DbPath { get; }

//     public GradingAppContext()
//     {
//         var path = Environment.CurrentDirectory;
//         DbPath = System.IO.Path.Join(path, "grading_app.db");
//     }

//     // The following configures EF to create a Sqlite database file in the
//     // special "local" folder for your platform.
//     protected override void OnConfiguring(DbContextOptionsBuilder options)
//         => options.UseSqlite($"Data Source={DbPath}");
// }

namespace GradingApp.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public List<Grade> Grades { get; } = new();
    }

    public class Grade
    {
        public int GradeId { get; set; }
        public required string Subject { get; set; }
        public int Score { get; set; }
        public DateOnly DateTaken { get; set; }
        public int StudentId { get; set; }
    }    
}
