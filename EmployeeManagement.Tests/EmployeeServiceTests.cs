using EmployeeManagement.Models;
using EmployeeManagement.Services;

namespace EmployeeManagement.Tests;

public class EmployeeServiceTests
{
    private static EmployeeService CreateService()
    {
        return new EmployeeService(storagePath: Path.Combine(Path.GetTempPath(), $"employee-tests-{Guid.NewGuid():N}.json"));
    }

    [Fact]
    public void AddEmployee_ShouldAddEmployee()
    {
        var service = CreateService();

        var employee = new Employee
        {
            EmployeeId = "E001",
            Name = "Alice Johnson",
            Email = "alice@example.com",
            Department = "Engineering",
            Designation = "Developer",
            JoiningDate = new DateTime(2024, 1, 10)
        };

        service.AddEmployee(employee);

        var result = service.SearchEmployeeById("E001");

        Assert.NotNull(result);
        Assert.Equal("Alice Johnson", result!.Name);
    }

    [Fact]
    public void AddEmployee_ShouldRejectDuplicateEmployeeId()
    {
        var service = CreateService();

        var first = new Employee { EmployeeId = "E001", Name = "A", Email = "a@example.com", Department = "IT", Designation = "Dev", JoiningDate = new DateTime(2024, 1, 1) };
        var second = new Employee { EmployeeId = "E001", Name = "B", Email = "b@example.com", Department = "IT", Designation = "Dev", JoiningDate = new DateTime(2024, 2, 1) };

        service.AddEmployee(first);
        var result = service.AddEmployee(second);

        Assert.False(result);
    }

    [Fact]
    public void SearchEmployeeById_ShouldReturnMatchingEmployee()
    {
        var service = CreateService();
        service.AddEmployee(new Employee { EmployeeId = "E002", Name = "Bob", Email = "bob@example.com", Department = "HR", Designation = "Manager", JoiningDate = new DateTime(2023, 6, 1) });

        var result = service.SearchEmployeeById("E002");

        Assert.NotNull(result);
        Assert.Equal("Bob", result!.Name);
    }

    [Fact]
    public void DeleteEmployee_ShouldRemoveEmployee()
    {
        var service = CreateService();
        service.AddEmployee(new Employee { EmployeeId = "E003", Name = "Carol", Email = "carol@example.com", Department = "Finance", Designation = "Analyst", JoiningDate = new DateTime(2022, 4, 1) });

        var result = service.DeleteEmployee("E003");

        Assert.True(result);
        Assert.Null(service.SearchEmployeeById("E003"));
    }

    [Fact]
    public void UpdateEmployee_ShouldChangeEmployeeDetails()
    {
        var service = CreateService();
        service.AddEmployee(new Employee { EmployeeId = "E004", Name = "Diana", Email = "diana@example.com", Department = "Sales", Designation = "Executive", JoiningDate = new DateTime(2021, 7, 1) });

        var updated = new Employee { EmployeeId = "E004", Name = "Diana Updated", Email = "diana.updated@example.com", Department = "Marketing", Designation = "Lead", JoiningDate = new DateTime(2023, 9, 15) };

        var result = service.UpdateEmployee(updated);

        Assert.True(result);
        var employee = service.SearchEmployeeById("E004");
        Assert.Equal("Diana Updated", employee!.Name);
        Assert.Equal("Marketing", employee.Department);
    }

    [Fact]
    public void IsValidEmail_ShouldReturnFalseForInvalidEmail()
    {
        var helper = new EmployeeManagement.Utilities.ValidationHelper();

        Assert.False(helper.IsValidEmail("not-an-email"));
        Assert.True(helper.IsValidEmail("user@example.com"));
    }
}
