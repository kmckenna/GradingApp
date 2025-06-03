using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GradingApp.Models;
using GradingApp.Controllers;
using GradingApp.Helpers;
using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Configuration.Binder;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.EnvironmentVariables; 

using System.Globalization;
using System.Threading.Tasks;


namespace GradingApp
{
    public class Program
    {
        private static readonly CultureInfo cultureInfo = new CultureInfo("en-US");
        private static async Task Main(string[] args)
        {

            string databasePath = Utilities.ConfigureDatabase();
            if (string.IsNullOrEmpty(databasePath))
            {
                Console.WriteLine("Database configuration is not properly configured. Please set environment variables.");
                return;
            }
            using var db = new GradingAppContext(databasePath);

            // Seed the database with initial data if empty
            await Utilities.SeedDatabase(db);

            // Display the header
            UI.DisplayHeader();

            // Display the options
            while (true)
            {
                UI.DisplayOptions();

                string input = Console.ReadLine() ?? string.Empty;

                Console.WriteLine();
                if (input == "1")
                {
                    await Students.AddStudent(db);
                }
                // else if (input == "2")
                // {
                //     await Students.UpdateStudent(db);
                // }
                // else if (input == "3")
                // {
                //     await Grades.AddGradeToStudent(db);
                // }
                // else if (input == "4")
                // {
                //     await Grades.DeleteGradeFromStudent(db);
                // }
                // else if (input == "5")
                // {
                //     await Students.GetStudentAverageGrade(db);
                // }
                // else if (input == "6")
                // {
                //     await Grades.DeleteStudent(db);
                // }                   
                // else if (input == "7")
                // {
                //     await Grades.DisplayAllStudentsAndGrades(db);
                // }
                // else if (input == "8")
                // {
                //     await Students.DisplayAllStudentsAverageGrades(db);
                // }
                // else if (input == "3")
                // {
                //     await Grades.GetStudentAverageGrade(db);
                // }
                // else if (input == "4")
                // {
                //     await Grades.DisplayStudentsAndGrades(db);
                // }
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

    }
}
