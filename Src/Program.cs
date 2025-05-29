using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GradingApp.Models;
using GradingApp.Controllers;
using GradingApp.Helpers;
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

            // Seed the database with initial data if empty
            await Data.SeedDatabase(db);

            // Display the header
            Data.DisplayHeader();

            // Display the options
            while (true)
            {
                Data.DisplayOptions();

                string input = Console.ReadLine() ?? string.Empty;

                if (input == "1")
                {
                    await Students.AddStudent(db);
                }
                else if (input == "2")
                {
                    await Students.AssignGradeToStudent(db);
                }
                else if (input == "3")
                {
                    await Grades.GetStudentAverageGrade(db);
                }
                else if (input == "4")
                {
                    await Grades.DisplayStudentsAndGrades(db);
                }
                else if (input == "99")
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
        // public static void DisplayOptions()
        // {
        //     Console.WriteLine();
        //     Console.WriteLine("Student Grades-select one of the following options:");
        //     Console.WriteLine("0. Seed Database");
        //     Console.WriteLine("1. Add Student");
        //     Console.WriteLine("2. Assign Grade to Student");
        //     Console.WriteLine("3. Calculate Students Average Grade");
        //     Console.WriteLine("4. Display All Students and Grades");
        //     Console.WriteLine("99. Exit");
        //     Console.Write("Select an option: ");
        // }



    }
}
