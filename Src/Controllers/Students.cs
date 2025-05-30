
using System.Globalization;
using GradingApp.Models;
using GradingApp.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GradingApp.Controllers
{
    public static class Students
    {
        private static readonly CultureInfo cultureInfo = new("en-US");
        private static ConsoleInput consoleInput;

        public static async Task AddStudent(GradingAppContext db)
        {
            // Prompt for student details
            Console.WriteLine("Adding a new student...");
            
            consoleInput = Utilities.IsInputValid("Student First Name", "string", "");
            if (!consoleInput.IsValid)
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            string? firstName = consoleInput.Input;

            consoleInput = Utilities.IsInputValid("Student Last Name", "string", "");
            if (!consoleInput.IsValid) 
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            string? lastName = consoleInput.Input;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("First name and last name cannot be empty. Please try again.");
                return;
            }
            // Create
            Console.WriteLine($"Inserting a new student: {firstName} {lastName}");
            db.Add(new Student { FirstName = firstName, LastName = lastName });
            await db.SaveChangesAsync();
        }
        public static async Task<Student?> GetStudentById(GradingAppContext db, int studentId, bool showConsole = false, bool showGrades = false)
        {
            // errors will still be shown in the console

            var student = await db.Students
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                Console.WriteLine($"\nNo student found with ID {studentId}.");
                Console.WriteLine("Please try again with a valid student ID.");
                return student;
            }
            
            if (showConsole)
            {
                Console.WriteLine($"\nStudent: {student.FirstName} {student.LastName} (ID: {student.StudentId})");
                if (showGrades)
                {
                    Console.WriteLine("Grades:");
                    if (student.Grades != null && student.Grades.Count > 0)
                    {
                        foreach (var grade in student.Grades)
                        {
                            Console.WriteLine($"  Subject: {grade.Subject}, Score: {grade.Score}, Date: {grade.DateTaken:MM/dd/yyyy}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("  No grades found for this student.");
                    }
                }
            }

            return student;
        }

        public static async Task AssignGradeToStudent(GradingAppContext db)
        {
            await Grades.DisplayStudentsAndGrades(db, false);

            consoleInput = Utilities.IsInputValid("Student ID", "int", "");
            if (!consoleInput.IsValid)  
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            int studentId = consoleInput.IntInput ?? 0;

            // Validate student ID
            var student = await GetStudentById(db, consoleInput.IntInput ?? 0, true, false);
            
            if (student == null)
            {
                Console.WriteLine($"No student found with ID {studentId}. Please try again.");
                return;
            }

            // Console.WriteLine($"Found student: {student.FirstName} {student.LastName} (ID: {student.StudentId})");
            consoleInput = Utilities.IsInputValid("Subject", "string", "");
            if (!consoleInput.IsValid) 
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            string? subject = consoleInput.Input;

            consoleInput = Utilities.IsInputValid("Score", "int", "");
            if (!consoleInput.IsValid) 
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            int? score = consoleInput.IntInput            
            if (score < 0 || score > 100)
            {
                Console.WriteLine("Score must be between 0 and 100. Please try again.");
                return;
            }

            consoleInput = Utilities.IsInputValid("Date Taken (MM/DD/YYYY)", "date", "");
            if (!consoleInput.IsValid) 
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            dateOnly? dateTaken = consoleInput.DateOnlyInput;
            if (dateTaken > DateOnly.FromDateTime(DateTime.Now))
            {
                Console.WriteLine("Date cannot be in the future. Please try again.");
                return;
            }

            Console.WriteLine($"\nAssigning grade ({subject} - {score} on {dateTaken}) to student: {student.FirstName} {student.LastName}");
            // Add the grade to the student's grades
            student.Grades.Add(
                new Grade { Subject = subject, Score = score, DateTaken = dateTaken });
            await db.SaveChangesAsync();

        }

    }
}