using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GradingApp.Models;
using System.Globalization;
using System.Threading.Tasks;

namespace GradingApp
{
    public class Program
    {
        private static readonly CultureInfo cultureInfo = new CultureInfo("en-US");
        private static async Task Main(string[] args)
        {
            using var db = new GradingAppContext();

            // Note: This sample requires the database to be created before running.
            Console.WriteLine($"Database path: {db.DbPath}.");

            Console.WriteLine("=========================================");
            Console.WriteLine("Welcome to the Grading App!");
            Console.WriteLine("=========================================");

            while (true)
            {
                DisplayOptions();

                string input = Console.ReadLine() ?? string.Empty;

                if (input == "1")
                {
                    await AddStudent(db);
                }
                else if (input == "2")
                {
                    await AssignGradeToStudent(db);
                }
                else if (input == "3")
                {
                    await GetStudentAverageGrade(db);
                }
                else if (input == "4")
                {
                    await DisplayStudentsAndGrades(db);
                }
                else if (input == "5")
                {
                    break;
                }

            }
            Console.WriteLine("Exiting program.");





            // // Update
            // Console.WriteLine("Updating the student and adding a Grade");
            // student.LastName = "Jones";
            // student.Grades.Add(
            //     new Grade { Subject = "Science", Score = 88, DateTaken = new DateOnly(2025, 5, 1) });
            // await db.SaveChangesAsync();

            // Delete
            // Console.WriteLine("Delete the student");
            // db.Remove(student);
            // await db.SaveChangesAsync();
        }
        public static void DisplayOptions()
        {
            Console.WriteLine();
            Console.WriteLine("Student Grades-select one of the following options:");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. Assign Grade to Student");
            Console.WriteLine("3. Calculate Students Average Grade");
            Console.WriteLine("4. Display All Students and Grades");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");
        }


        public static async Task AddStudent(GradingAppContext db)
        {
            Console.Write("Enter student's first name: ");
            string? firstName = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(firstName))
            {
                Console.WriteLine("First name cannot be empty. Please try again.");
                return;
            }
            Console.Write("Enter student's last name: ");
            string? lastName = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Last name cannot be empty. Please try again.");
                return;
            }
            // Create
            Console.WriteLine($"Inserting a new student: {firstName} {lastName}");
            db.Add(new Student { FirstName = firstName, LastName = lastName });
            await db.SaveChangesAsync();        
        }

        public static async Task AssignGradeToStudent(GradingAppContext db)
        {
            await DisplayStudentsAndGrades(db, false);

            Console.Write("Enter student ID: ");
            string? studentIdInput = Console.ReadLine();
            if (studentIdInput == null) {
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

            Console.Write("Enter subject: ");
            string? subject = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(subject))
            {
                Console.WriteLine("Subject cannot be empty. Please try again.");
                return;
            }

            Console.Write("Enter score: ");
            string? scoreInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(scoreInput) || !int.TryParse(scoreInput, out int score))
            {
                Console.WriteLine("Score must be a valid integer. Please try again.");
                return;
            }   
            if (score < 0 || score > 100)
            {
                Console.WriteLine("Score must be between 0 and 100. Please try again.");
                return;
            }


            
            Console.Write("Enter date taken (MM/DD/YYYY): ");
            string? dateInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(dateInput) || !DateOnly.TryParse(dateInput, cultureInfo, DateTimeStyles.None, out DateOnly dateTaken))
            {
                Console.WriteLine("Date must be in the format MM/DD/YYYY. Please try again.");
                return;
            }
            if (dateTaken >  DateOnly.FromDateTime(DateTime.Now))
            {
                Console.WriteLine("Date cannot be in the future. Please try again.");
                return;
            }
            
            Console.WriteLine($"Assigning grade to student: {student.FirstName} {student.LastName}");
            Console.WriteLine($"Inserting a new grade for student {student.StudentId}: {subject} - {score} on {dateTaken}");    
            student.Grades.Add(
                new Grade { Subject = subject, Score = score, DateTaken = dateTaken });
            await db.SaveChangesAsync(); 
                
        }

        public static async Task GetStudentAverageGrade(GradingAppContext db)
        {
            await DisplayStudentsAndGrades(db, false);

            Console.Write("Enter student ID: ");
            string? studentIdInput = Console.ReadLine();
            if (studentIdInput == null) {
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
            Console.WriteLine("Students:");
            foreach (var student in students)
            {
                
                Console.WriteLine($"  Name: {student.FirstName} {student.LastName} (StudentID: {student.StudentId})");
                if (showGrades == true)
                {
                    Console.WriteLine("  Grades:");
                    if (student.Grades.Any() != true)
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
            Console.WriteLine("") ;
        }        
    }        
}
