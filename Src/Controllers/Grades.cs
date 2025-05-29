using System.Globalization;
using GradingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GradingApp.Controllers
{
    public static class Grades
    {
        private static readonly CultureInfo cultureInfo = new("en-US");
    
        public static async Task GetStudentAverageGrade(GradingAppContext db)
        {
            await DisplayStudentsAndGrades(db, false);

            Console.Write("Enter student ID: ");
            string? studentIdInput = Console.ReadLine();
            if (studentIdInput == null)
            {
                Console.WriteLine("Student ID cannot be empty. Please try again.");
                return;
            }

            int studentId = int.Parse(studentIdInput);

            Console.WriteLine("Querying for a student");
            var student = await db.Students
                .OrderBy(b => b.StudentId)
                .FirstAsync();

            if (student == null || student.StudentId != studentId)
            {
                Console.WriteLine($"No student found with ID {studentId}. Please try again.");
                return;
            }
            Console.WriteLine($"Found student: {student.FirstName} {student.LastName} (ID: {student.StudentId})");
            if (student.Grades.Count == 0)
            {
                Console.WriteLine($"No grades available for student {student.FirstName} {student.LastName}.");
                return;
            }
            Console.WriteLine("Grades:");
            foreach (var grade in student.Grades)
            {
                Console.WriteLine($"  Subject: {grade.Subject}, Score: {grade.Score}");
            }
            double average = student.Grades.Average(g => g.Score);
            Console.WriteLine($"Average grade for student {student.FirstName} {student.LastName} (ID: {student.StudentId}) is: {average:F2}");

        }

        // public static void GetStudentGrades(GradingAppContext db)
        // {
        //     DisplayStudentsAndGrades(db, false);
        //     Console.Write("Enter student ID: ");
        //     int studentId = int.Parse(Console.ReadLine() ?? string.Empty);
        //     Console.WriteLine($"Retrieving grades for student ID {studentId}.");
        // }

        public static async Task DisplayStudentsAndGrades(GradingAppContext db, bool showGrades = true)
        {
            Console.WriteLine("Displaying all students and their grades:");
            var students = await db.Students
                .Include(s => s.Grades)
                .OrderBy(s => s.LastName)
                .ToListAsync();

            // int numberOfStudents = students.Length;
            // Console.WriteLine($"Number of students: {numberOfStudents}");
            if (students.Count == 0)
            {
                Console.WriteLine("No students found. Enter option 0 to seed data or enter option 1 to add a student.");
                return;
            }
            Console.WriteLine("Students:");
            foreach (var student in students)
            {

                Console.WriteLine($"  Name: {student.FirstName} {student.LastName} (StudentID: {student.StudentId})");
                if (showGrades == true)
                {
                    Console.WriteLine("  Grades:");
                    if (student.Grades.Count == 0)
                    {
                        Console.WriteLine("    No grades available.");
                    }
                    else
                    {
                        foreach (var grade in student.Grades)
                        {
                            Console.WriteLine($"    Subject: {grade.Subject} - {grade.Score} ({grade.DateTaken.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)})");
                        }
                    }
                }
            }
            Console.WriteLine("");
        }
    }    
}