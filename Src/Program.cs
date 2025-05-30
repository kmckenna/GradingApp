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
            // Console.WriteLine("base dir: " + AppContext.BaseDirectory);
            // Console.WriteLine("env current dir: " + Environment.CurrentDirectory);
            // Console.WriteLine("appdomain base dir: " + AppDomain.CurrentDomain.BaseDirectory);

            // Configure the application
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

                        
            /// <summary>
            /// Combines the root directory, database directory, and database file name into a single path string representing the location of the database file.
            /// </summary>
            /// <param name="rootDirectory">The root directory of the application.</param>
            /// <param name="databaseDirectory">The subdirectory where the database is stored.</param>
            /// <param name="databaseFileName">The name of the database file.</param>
            /// <returns>The full file path to the database.</returns>
            var rootDirectory = configuration.GetSection("Settings:RootDirectory").Value;
            if (string.IsNullOrEmpty(rootDirectory))
            {
                Console.WriteLine("Configuration setting for root directory is missing or invalid.");
            }
            // Console.WriteLine($"Root directory: {rootDirectory}");

            var databaseDirectory = configuration.GetSection("Settings:Database:DatabaseDirectory").Value;
            if (string.IsNullOrEmpty(databaseDirectory))
            {
                Console.WriteLine("Configuration setting for database directory is missing or invalid.");
            }
            // Console.WriteLine($"Database directory: {databaseDirectory}");

            var databaseFileName = configuration.GetSection("Settings:Database:DatabaseFileName").Value;
            if (string.IsNullOrEmpty(databaseFileName))
            {
                Console.WriteLine("Configuration setting for database file name is missing or invalid.");
            }
            // Console.WriteLine($"Database file name: {databaseFileName}");

            if (string.IsNullOrEmpty(rootDirectory) || string.IsNullOrEmpty(databaseDirectory) || string.IsNullOrEmpty(databaseFileName))
            {
                Console.WriteLine("Configuration settings for database path are missing or invalid.");
                return;
            }

            var fullQualifiedNameDatabaseDirectoryName = System.IO.Path.Combine(rootDirectory, databaseDirectory);
            
            // Ensure the directory exists  
            if (!System.IO.Directory.Exists(fullQualifiedNameDatabaseDirectoryName))
            {
                Console.WriteLine($"Creating directory: {fullQualifiedNameDatabaseDirectoryName}");
                try
                {
                    // Attempt to create the directory
                    System.IO.Directory.CreateDirectory(fullQualifiedNameDatabaseDirectoryName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating directory: {ex.Message}");
                    return;
                }
                
            }

            var databasePath = System.IO.Path.Combine(rootDirectory, databaseDirectory, databaseFileName);
            using var db = new GradingAppContext(databasePath);

            // Note: This sample requires the database to be created before running.
            // Console.WriteLine($"Database path: {db.DbPath}.");

            // Seed the database with initial data if empty
            await Data.SeedDatabase(db);

            // Display the header
            Data.DisplayHeader();

            // Display the options
            while (true)
            {
                Data.DisplayOptions();

                string input = Console.ReadLine() ?? string.Empty;

                Console.WriteLine();
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

    }
}
