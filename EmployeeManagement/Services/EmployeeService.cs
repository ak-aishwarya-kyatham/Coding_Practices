using System.Text.Json;
using EmployeeManagement.Models;
using EmployeeManagement.Utilities;

namespace EmployeeManagement.Services;

public class EmployeeService
{
    private readonly List<Employee> _employees = [];
    private readonly string _storagePath;
    private readonly ValidationHelper _validationHelper = new();

    public EmployeeService(string? storagePath = null)
    {
        _storagePath = storagePath ?? Path.Combine(AppContext.BaseDirectory, "employees.json");
        LoadEmployees();
    }

    public bool AddEmployee(Employee employee)
    {
        if (employee is null)
        {
            return false;
        }

        if (_employees.Any(e => string.Equals(e.EmployeeId, employee.EmployeeId, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        _employees.Add(employee);
        SaveEmployees();
        return true;
    }

    public List<Employee> ViewEmployees() => _employees.OrderBy(e => e.Name).ToList();

    public Employee? SearchEmployeeById(string employeeId)
    {
        return _employees.FirstOrDefault(e => string.Equals(e.EmployeeId, employeeId, StringComparison.OrdinalIgnoreCase));
    }

    public bool UpdateEmployee(Employee updatedEmployee)
    {
        var existing = SearchEmployeeById(updatedEmployee.EmployeeId);
        if (existing is null)
        {
            return false;
        }

        existing.Name = updatedEmployee.Name;
        existing.Email = updatedEmployee.Email;
        existing.Department = updatedEmployee.Department;
        existing.Designation = updatedEmployee.Designation;
        existing.JoiningDate = updatedEmployee.JoiningDate;
        SaveEmployees();
        return true;
    }

    public bool DeleteEmployee(string employeeId)
    {
        var employee = SearchEmployeeById(employeeId);
        if (employee is null)
        {
            return false;
        }

        _employees.Remove(employee);
        SaveEmployees();
        return true;
    }

    public List<Employee> SortEmployeesByName() => _employees.OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase).ToList();

    public List<Employee> FilterByDepartment(string department)
    {
        return _employees
            .Where(e => string.Equals(e.Department, department, StringComparison.OrdinalIgnoreCase))
            .OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public void LoadEmployees()
    {
        if (!File.Exists(_storagePath))
        {
            return;
        }

        try
        {
            var json = File.ReadAllText(_storagePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            var employees = JsonSerializer.Deserialize<List<Employee>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (employees is not null)
            {
                _employees.Clear();
                _employees.AddRange(employees);
            }
        }
        catch (Exception)
        {
            _employees.Clear();
        }
    }

    public void SaveEmployees()
    {
        try
        {
            var directory = Path.GetDirectoryName(_storagePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(_employees, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_storagePath, json);
        }
        catch (Exception)
        {
            // Fail gracefully; no crash in console app.
        }
    }
}
