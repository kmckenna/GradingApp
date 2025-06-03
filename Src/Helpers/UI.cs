using System.Globalization;
using GradingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GradingApp.Helpers
{
    public static class UI
    {
        public static void DisplayOptions()
        {
            Console.WriteLine();
            Console.WriteLine("=========================================");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Add a new student");
            Console.WriteLine("2. Update a student");
            Console.WriteLine("3. Add a grade to a student");
            Console.WriteLine("4. Delete a grade to a student");
            Console.WriteLine("5. Get a student's average grade");
            Console.WriteLine("6. Delete a student");
            Console.WriteLine("7. Display all students and their grades");
            Console.WriteLine("8. Display all students and their average grades");
            // Console.WriteLine("4. Display all students and their grades");
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

    }
}