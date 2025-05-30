using System.Globalization;
using GradingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GradingApp.Helpers
{
    public static class Data
    {
        public static void DisplayOptions()
        {
            Console.WriteLine();
            Console.WriteLine("=========================================");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Add a new student");
            Console.WriteLine("2. Assign a grade to a student");
            Console.WriteLine("3. Get a student's average grade");
            Console.WriteLine("4. Display all students and their grades");
            Console.WriteLine("99. Exit");
            Console.WriteLine("=========================================");
            Console.WriteLine();
        }
        public static void DisplayHeader()
        {
            Console.WriteLine();
            Console.WriteLine("=========================================");
            Console.WriteLine("Welcome to the Student Grading System");
            Console.WriteLine("=========================================");
            Console.WriteLine();
        }

        public static async Task SeedDatabase(GradingAppContext db)
        {
            db.Database.Migrate();

            var students = await db.Students
                .Include(s => s.Grades)
                .OrderBy(s => s.LastName)
                .ToListAsync();

            if (students.Count == 0)
            {
                // If no students found, add initial data
                Console.WriteLine("Seeding the database with initial data...");
                db.Students.AddRange(
                    new Student { FirstName = "John", LastName = "Doe", Grades = { new Grade { Subject = "Math", Score = 95, DateTaken = new DateOnly(2025, 1, 15) } } },
                    new Student { FirstName = "Jane", LastName = "Smith", Grades = { new Grade { Subject = "Science", Score = 88, DateTaken = new DateOnly(2025, 2, 20) } } },
                    new Student { FirstName = "Alice", LastName = "Johnson", Grades = { new Grade { Subject = "History", Score = 92, DateTaken = new DateOnly(2025, 3, 10) } } },
                    new Student { FirstName = "Bob", LastName = "Brown", Grades = { new Grade { Subject = "English", Score = 85, DateTaken = new DateOnly(2025, 4, 5) } } }
                );
            }
            // else if (students.Count < 4)
            // {

            // }

            await db.SaveChangesAsync();
        }
    }
}