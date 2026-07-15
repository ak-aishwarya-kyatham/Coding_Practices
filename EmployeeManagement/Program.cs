using EmployeeManagement.Models;
using EmployeeManagement.Services;
using EmployeeManagement.Utilities;

namespace EmployeeManagement;

public static class Program
{
    public static void Main()
    {
        var service = new EmployeeService();
        var validationHelper = new ValidationHelper();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("Employee Management System");
            Console.WriteLine("==============================");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View All Employees");
            Console.WriteLine("3. Search Employee by ID");
            Console.WriteLine("4. Update Employee");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Sort Employees by Name");
            Console.WriteLine("7. Filter Employees by Department");
            Console.WriteLine("8. Exit");
            Console.WriteLine("==============================");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddEmployee(service, validationHelper);
                    break;
                case "2":
                    ViewEmployees(service);
                    break;
                case "3":
                    SearchEmployee(service);
                    break;
                case "4":
                    UpdateEmployee(service, validationHelper);
                    break;
                case "5":
                    DeleteEmployee(service);
                    break;
                case "6":
                    SortEmployees(service);
                    break;
                case "7":
                    FilterEmployees(service);
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Pause();
                    break;
            }
        }
    }

    private static void AddEmployee(EmployeeService service, ValidationHelper validationHelper)
    {
        Console.WriteLine();
        var employee = new Employee
        {
            EmployeeId = validationHelper.ReadEmployeeId("Enter Employee ID: "),
            Name = validationHelper.ReadRequiredString("Enter Name: "),
            Email = ReadValidEmail(validationHelper),
            Department = validationHelper.ReadRequiredString("Enter Department: "),
            Designation = validationHelper.ReadRequiredString("Enter Designation: "),
            JoiningDate = validationHelper.ReadValidDate("Enter Joining Date (yyyy-MM-dd): ")
        };

        if (service.AddEmployee(employee))
        {
            Console.WriteLine("Employee added successfully.");
        }
        else
        {
            Console.WriteLine("Employee ID already exists. Please use a unique ID.");
        }

        Pause();
    }

    private static void ViewEmployees(EmployeeService service)
    {
        Console.WriteLine();
        var employees = service.ViewEmployees();
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            Pause();
            return;
        }

        Console.WriteLine("{0,-12} {1,-20} {2,-25} {3,-15} {4,-20} {5,-15}", "EmployeeId", "Name", "Email", "Department", "Designation", "JoiningDate");
        Console.WriteLine(new string('-', 120));
        foreach (var employee in employees)
        {
            Console.WriteLine("{0,-12} {1,-20} {2,-25} {3,-15} {4,-20} {5,-15:yyyy-MM-dd}", employee.EmployeeId, employee.Name, employee.Email, employee.Department, employee.Designation, employee.JoiningDate);
        }

        Pause();
    }

    private static void SearchEmployee(EmployeeService service)
    {
        Console.WriteLine();
        Console.Write("Enter Employee ID: ");
        var employeeId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(employeeId))
        {
            Console.WriteLine("Employee ID cannot be empty.");
            Pause();
            return;
        }

        var employee = service.SearchEmployeeById(employeeId.Trim());
        if (employee is null)
        {
            Console.WriteLine("Employee not found.");
        }
        else
        {
            Console.WriteLine($"Employee ID: {employee.EmployeeId}");
            Console.WriteLine($"Name: {employee.Name}");
            Console.WriteLine($"Email: {employee.Email}");
            Console.WriteLine($"Department: {employee.Department}");
            Console.WriteLine($"Designation: {employee.Designation}");
            Console.WriteLine($"Joining Date: {employee.JoiningDate:yyyy-MM-dd}");
        }

        Pause();
    }

    private static void UpdateEmployee(EmployeeService service, ValidationHelper validationHelper)
    {
        Console.WriteLine();
        Console.Write("Enter Employee ID to update: ");
        var employeeId = Console.ReadLine();

        var existing = service.SearchEmployeeById(employeeId?.Trim() ?? string.Empty);
        if (existing is null)
        {
            Console.WriteLine("Employee not found.");
            Pause();
            return;
        }

        var updatedEmployee = new Employee
        {
            EmployeeId = existing.EmployeeId,
            Name = validationHelper.ReadRequiredString("Enter New Name: "),
            Email = ReadValidEmail(validationHelper),
            Department = validationHelper.ReadRequiredString("Enter New Department: "),
            Designation = validationHelper.ReadRequiredString("Enter New Designation: "),
            JoiningDate = validationHelper.ReadValidDate("Enter New Joining Date (yyyy-MM-dd): ")
        };

        if (service.UpdateEmployee(updatedEmployee))
        {
            Console.WriteLine("Employee updated successfully.");
        }
        else
        {
            Console.WriteLine("Employee could not be updated.");
        }

        Pause();
    }

    private static void DeleteEmployee(EmployeeService service)
    {
        Console.WriteLine();
        Console.Write("Enter Employee ID to delete: ");
        var employeeId = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(employeeId))
        {
            Console.WriteLine("Employee ID cannot be empty.");
            Pause();
            return;
        }

        Console.Write("Are you sure you want to delete this employee? (y/n): ");
        var confirmation = Console.ReadLine();
        if (!string.Equals(confirmation, "y", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Deletion cancelled.");
            Pause();
            return;
        }

        var deleted = service.DeleteEmployee(employeeId.Trim());
        if (deleted)
        {
            Console.WriteLine("Employee deleted successfully.");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }

        Pause();
    }

    private static void SortEmployees(EmployeeService service)
    {
        Console.WriteLine();
        var employees = service.SortEmployeesByName();
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            Pause();
            return;
        }

        foreach (var employee in employees)
        {
            Console.WriteLine($"{employee.Name} ({employee.EmployeeId})");
        }

        Pause();
    }

    private static void FilterEmployees(EmployeeService service)
    {
        Console.WriteLine();
        Console.Write("Enter Department: ");
        var department = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(department))
        {
            Console.WriteLine("Department cannot be empty.");
            Pause();
            return;
        }

        var employees = service.FilterByDepartment(department.Trim());
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found in this department.");
            Pause();
            return;
        }

        foreach (var employee in employees)
        {
            Console.WriteLine($"{employee.EmployeeId} - {employee.Name} - {employee.Designation}");
        }

        Pause();
    }

    private static string ReadValidEmail(ValidationHelper validationHelper)
    {
        while (true)
        {
            var email = validationHelper.ReadRequiredString("Enter Email: ");
            if (validationHelper.IsValidEmail(email))
            {
                return email;
            }

            Console.WriteLine("Please enter a valid email address.");
        }
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}
