
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
            ErrorMessage = string.Empty;

        }
    }
    
    public static class Utilities
    {

        public static ConsoleInput IsInputValid(string fieldName, string? fieldType, string? fieldFormat)
        {
            ConsoleInput consoleInput = new(fieldName, fieldType, fieldFormat);

            Console.Write($"Enter {fieldName}: ");
            consoleInput.Input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(consoleInput.Input))
            {
                consoleInput.ErrorMessage = $"{fieldName} cannot be empty.";
                return consoleInput;
            }

            if (fieldType == "int")
            {
                if (!int.TryParse(consoleInput.Input, out int IntInput) || IntInput <= 0)
                {
                    consoleInput.ErrorMessage = $"{fieldName} must be a valid integer greater than 0.";
                    return consoleInput;
                }
                consoleInput.IntInput = IntInput;
            }
            else if (fieldType == "date")
            {
                if (!DateOnly.TryParseExact(consoleInput.Input, fieldFormat, out DateOnly DateOnlyInput))
                {
                    consoleInput.ErrorMessage = $"{fieldName} must be a valid date in the format {fieldFormat}.";
                    return consoleInput;
                }
                consoleInput.DateOnlyInput = DateOnlyInput;
            }


            consoleInput.IsValid = true;
            return consoleInput;

        }
    }
}