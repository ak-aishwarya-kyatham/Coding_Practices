namespace EmployeeManagement.Utilities;

public class ValidationHelper
{
    public bool IsNullOrWhiteSpace(string? value) => string.IsNullOrWhiteSpace(value);

    public bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email.Trim();
        }
        catch
        {
            return false;
        }
    }

    public string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            Console.WriteLine("Input cannot be empty.");
        }
    }

    public string ReadEmployeeId(string prompt)
    {
        while (true)
        {
            var input = ReadRequiredString(prompt);
            if (input.Length >= 1)
            {
                return input;
            }

            Console.WriteLine("Employee ID cannot be empty.");
        }
    }

    public DateTime ReadValidDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (DateTime.TryParse(input, out var parsedDate))
            {
                return parsedDate;
            }

            Console.WriteLine("Please enter a valid date.");
        }
    }
}
