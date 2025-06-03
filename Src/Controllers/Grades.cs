using System.Globalization;
using GradingApp.Controllers;
using GradingApp.Models;
using GradingApp.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GradingApp.Controllers
{
    public static class Grades
    {
        private static readonly CultureInfo cultureInfo = new("en-US");
        private static ConsoleInput consoleInput;
        public static async Task AddGrade(GradingAppContext db, Student student)
        {
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.WriteLine($"Adding grade for student {student.FirstName} {student.LastName} (ID: {student.StudentId})");

            consoleInput = Utilities.IsInputValid("Subject", "string", "");
            if (!consoleInput.IsValid)
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            string subject = consoleInput.Input == null ? "" : consoleInput.Input;

            consoleInput = Utilities.IsInputValid("Score", "int", "");
            if (!consoleInput.IsValid)
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            int score = consoleInput.IntInput == null ? 0 : consoleInput.IntInput.Value;
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
            DateOnly dateTaken = consoleInput.DateOnlyInput;
            if (dateTaken > DateOnly.FromDateTime(DateTime.Now))
            {
                Console.WriteLine("Date cannot be in the future. Please try again.");
                return;
            }

            // // Create a new grade and add it to the student's grades
            // if (student.Grades == null)
            // {
            //     student.Grades = new List<Grade>();
            // }
            // student.Grades.Add( 
            //         new Grade { Subject = subject, Score = score, DateTaken = dateTaken });

            // Save the changes to the database
            Console.WriteLine($"\nAssigning grade ({subject} - {score} on {dateTaken}) to student: {student.FirstName} {student.LastName}");
            // Add the grade to the student's grades
            student.Grades.Add(
                new Grade { Subject = subject, Score = score, DateTaken = dateTaken });
            await db.SaveChangesAsync();            

            // Console.WriteLine($"Grade added successfully for student {student.FirstName} {student.LastName} (ID: {student.StudentId}).");
        }
        public static void GetStudentAverageGrade(Student student)
        {
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            if (student.Grades == null || student.Grades.Count == 0)
            {
                Console.WriteLine($"No grades available for student {student.FirstName} {student.LastName} (ID: {student.StudentId}).");
                return;
            }
            Console.WriteLine($"Found student: {student.FirstName} {student.LastName} (ID: {student.StudentId})");
            Console.WriteLine("Calculating average grade...");

            double average = student.Grades.Average(g => g.Score);
            Console.WriteLine($"Average grade for student {student.FirstName} {student.LastName} (ID: {student.StudentId}) is: {average:F2} out of {student.Grades.Count} grades.");

            return;

        }

        public static void DisplayAllGradesByStudent(Student student)
        {
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }
            Console.WriteLine($"Grades for student {student.FirstName} {student.LastName} (ID: {student.StudentId}):");
            if (student.Grades == null || student.Grades.Count == 0)
            {
                Console.WriteLine("No grades available for this student.");
                return;
            }

            foreach (var grade in student.Grades)
            {
                DisplayGrade(grade);
            }
        }
        public static void DisplayGrade(Grade grade)
        {
            if (grade == null)
            {
                Console.WriteLine("Grade not found.");
                return;
            }

            Console.WriteLine($"Subject: {grade.Subject} - {grade.Score} ({grade.DateTaken.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}) {grade.GradeId}");
        }
    }    
}