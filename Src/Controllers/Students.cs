
using System.Globalization;
using GradingApp.Controllers;
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
        public static async Task UpdateStudent(GradingAppContext db)
        {
            // Display all students
            await DisplayStudentsAndGrades(db, false);

            consoleInput = Utilities.IsInputValid("Student ID", "int", "");
            if (!consoleInput.IsValid)  
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            int studentId = consoleInput.IntInput ?? 0;
            if (studentId <= 0)
            {
                Console.WriteLine("Invalid student ID. Please try again.");
                return;
            }

            // Validate student ID
            var student = await GetStudentById(db, studentId, true, false);
            
            if (student == null)
            {
                Console.WriteLine($"No student found with ID {studentId}. Please try again.");
                return;
            }

            // Prompt for new details
            consoleInput = Utilities.IsInputValid("New First Name", "string", "");
            if (!consoleInput.IsValid) 
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            string? newFirstName = consoleInput.Input;

            consoleInput = Utilities.IsInputValid("New Last Name", "string", "");
            if (!consoleInput.IsValid) 
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            string? newLastName = consoleInput.Input;

            if (string.IsNullOrWhiteSpace(newFirstName) || string.IsNullOrWhiteSpace(newLastName))
            {
                Console.WriteLine("First name and last name cannot be empty. Please try again.");
                return;
            }

            // Update
            Console.WriteLine($"Updating student ID {studentId} to: {newFirstName} {newLastName}");
            student.FirstName = newFirstName;
            student.LastName = newLastName;
            await db.SaveChangesAsync();
        }
        public static async Task GetStudentAverageGrade(GradingAppContext db)
        {
            await DisplayStudentsAndGrades(db, false);

            consoleInput = Utilities.IsInputValid("Student ID", "int", "");
            if (!consoleInput.IsValid)
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            int studentId = consoleInput.IntInput ?? 0;

            // Validate student ID
            var student = await GetStudentById(db, studentId, true, false);

            if (student == null)
            {
                Console.WriteLine($"No student found with ID {studentId}. Please try again.");
                return;
            }

            if (student.Grades.Count == 0)
            {
                Console.WriteLine($"No grades available for student {student.FirstName} {student.LastName}.");
                return;
            }

            double average = student.Grades.Average(g => g.Score);
            Console.WriteLine($"Average grade for student {student.FirstName} {student.LastName} (ID: {student.StudentId}) is: {average:F2}");
            return;
        }       
        public static async Task<Student?> GetStudentById(GradingAppContext db, int studentId, bool showConsole = false, bool showGrades = false)
        {
            // errors will still be shown in the console

            var student = await db.Students
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                Console.WriteLine($"\nNo student found with ID {studentId}. Please try again.");
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
            await DisplayStudentsAndGrades(db, false);

            consoleInput = Utilities.IsInputValid("Student ID", "int", "");
            if (!consoleInput.IsValid)  
            {
                Console.WriteLine(consoleInput.ErrorMessage);
                return;
            }
            int studentId = consoleInput.IntInput ?? 0;
            if (studentId <= 0)
            {
                Console.WriteLine("Invalid student ID. Please try again.");
                return;
            }

            // Validate student ID
            var student = await GetStudentById(db, consoleInput.IntInput ?? 0, true, false);
            
            if (student == null)
            {
                Console.WriteLine($"No student found with ID {studentId}. Please try again.");
                return;
            }
            await Grades.AddGrade(db, student);

        }

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
                Console.WriteLine($"Name: {student.FirstName} {student.LastName} (StudentID: {student.StudentId})");
                if (showGrades == true)
                {
                    if (student.Grades.Count == 0)
                    {
                        Console.WriteLine("No grades available.");
                    }
                    else
                    {
                        Grades.DisplayAllGradesByStudent(student);
                    }
                    Console.WriteLine();
                }

            }
            Console.WriteLine();
        }
    }
}