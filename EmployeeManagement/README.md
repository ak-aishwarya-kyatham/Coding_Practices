# EmployeeManagement

## Project Overview
EmployeeManagement is a menu-driven console application built with C# and .NET 8 for managing employee records. It demonstrates clean architecture, validation, persistence, and test coverage for an internship evaluation project.

## Features
- Add employees with validated input
- View all employees in a formatted table
- Search employees by ID
- Update employee details without changing the ID
- Delete employees with confirmation
- Sort employees by name
- Filter employees by department
- Persist data to employees.json

## Folder Structure
- Models/Employee.cs
- Services/EmployeeService.cs
- Utilities/ValidationHelper.cs
- Program.cs
- employees.json

## Technologies Used
- C#
- .NET 8
- xUnit
- System.Text.Json

## How to Run
1. Open the solution in Visual Studio 2022 or Visual Studio Code.
2. Restore packages.
3. Build and run the EmployeeManagement project.
4. Use the menu to manage employees.

## Validation Rules
- Employee ID: required and unique
- Name: required
- Email: must be a valid email address
- Department: required
- Designation: required
- Joining Date: must be a valid date

## Coding Best Practices Followed
- Separation of concerns
- SOLID-inspired structure
- Reusable validation logic
- Guard clauses and clear method responsibilities
- Graceful exception handling

## Screenshots
- Placeholder: Add screenshots of the console UI here.

## Future Enhancements
- Add authentication for admin users
- Implement advanced search and filtering
- Support exporting employee data to CSV
- Add more comprehensive unit tests
