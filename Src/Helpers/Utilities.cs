using System.Globalization;
using GradingApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Configuration.Binder;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.EnvironmentVariables; 

namespace GradingApp.Helpers
{
    public struct ConsoleInput
    {
        public string FieldName { get; set; }
        public string? FieldType { get; set; }
        public string? FieldFormat { get; set; }
        public string? Input { get; set; }
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }

        public int? IntInput { get; set; }
        public DateOnly DateOnlyInput { get; set; }
        public ConsoleInput(string fieldName, string? fieldType, string? fieldFormat)
        {
            FieldName = fieldName;
            FieldType = fieldType;
            FieldFormat = fieldFormat;

            IsValid = false;
            ErrorMessage = "    ";

        }
    }

    public static class Utilities
    {

        public static ConsoleInput IsInputValid(string fieldName, string? fieldType, string? fieldFormat)
        {
            ConsoleInput consoleInput = new(fieldName, fieldType, fieldFormat);
            Console.WriteLine("=========================================");
            Console.Write($"Enter {fieldName}: ");
            string rawInput = Console.ReadLine() ?? string.Empty;
            consoleInput.Input = rawInput.Trim();

            // Console.WriteLine();

            if (string.IsNullOrEmpty(consoleInput.Input))
            {
                consoleInput.ErrorMessage += $"{fieldName} cannot be empty or whitespace. Please try again.";
                return consoleInput;
            }

            if (string.IsNullOrEmpty(consoleInput.Input))
            {
                consoleInput.ErrorMessage += $"{fieldName} cannot be empty. Please try again.";
                return consoleInput;
            }

            if (fieldType == "int")
            {
                if (!int.TryParse(consoleInput.Input, out int IntInput) || IntInput <= 0)
                {
                    consoleInput.ErrorMessage += $"{fieldName} must be a valid integer greater than 0. Please try again.";
                    return consoleInput;
                }
                consoleInput.IntInput = IntInput;
            }
            else if (fieldType == "date")
            {
                if (!DateOnly.TryParseExact(consoleInput.Input, fieldFormat, out DateOnly DateOnlyInput))
                {
                    consoleInput.ErrorMessage += $"{fieldName} must be a valid date in the format {fieldFormat}. Please try again.";
                    return consoleInput;
                }
                consoleInput.DateOnlyInput = DateOnlyInput;
            }


            consoleInput.IsValid = true;

            return consoleInput;

        }
        public static string ConfigureDatabase()
        {
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
            /// 
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
                return string.Empty;
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
                    return string.Empty;
                }
                
            }

            return System.IO.Path.Combine(rootDirectory, databaseDirectory, databaseFileName);            
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