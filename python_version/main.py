import os
from datetime import date

from employee_service import EmployeeService
from models import Employee
from validation_helper import ValidationHelper


def clear_screen():
    os.system("cls" if os.name == "nt" else "clear")


def pause():
    input("\nPress Enter to continue...")


def read_valid_email(validation_helper):
    while True:
        email = validation_helper.read_required_string("Enter Email: ")
        if validation_helper.is_valid_email(email):
            return email
        print("Please enter a valid email address.")


def add_employee(service, validation_helper):
    print()
    employee = Employee(
        employee_id=validation_helper.read_employee_id("Enter Employee ID: "),
        name=validation_helper.read_required_string("Enter Name: "),
        email=read_valid_email(validation_helper),
        department=validation_helper.read_required_string("Enter Department: "),
        designation=validation_helper.read_required_string("Enter Designation: "),
        joining_date=validation_helper.read_valid_date("Enter Joining Date (yyyy-MM-dd): "),
    )

    if service.add_employee(employee):
        print("Employee added successfully.")
    else:
        print("Employee ID already exists. Please use a unique ID.")

    pause()


def view_employees(service):
    print()
    employees = service.view_employees()
    if not employees:
        print("No employees found.")
        pause()
        return

    print(f"{'EmployeeId':<12} {'Name':<20} {'Email':<25} {'Department':<15} {'Designation':<20} {'JoiningDate':<15}")
    print("-" * 120)
    for employee in employees:
        joining_date = employee.joining_date.strftime("%Y-%m-%d") if isinstance(employee.joining_date, date) else str(employee.joining_date or "")
        print(f"{employee.employee_id:<12} {employee.name:<20} {employee.email:<25} {employee.department:<15} {employee.designation:<20} {joining_date:<15}")

    pause()


def search_employee(service):
    print()
    employee_id = input("Enter Employee ID: ").strip()
    if not employee_id:
        print("Employee ID cannot be empty.")
        pause()
        return

    employee = service.search_employee_by_id(employee_id)
    if employee is None:
        print("Employee not found.")
    else:
        print(f"Employee ID: {employee.employee_id}")
        print(f"Name: {employee.name}")
        print(f"Email: {employee.email}")
        print(f"Department: {employee.department}")
        print(f"Designation: {employee.designation}")
        join_date = employee.joining_date.strftime("%Y-%m-%d") if isinstance(employee.joining_date, date) else str(employee.joining_date or "")
        print(f"Joining Date: {join_date}")

    pause()


def update_employee(service, validation_helper):
    print()
    employee_id = input("Enter Employee ID to update: ").strip()
    existing = service.search_employee_by_id(employee_id)
    if existing is None:
        print("Employee not found.")
        pause()
        return

    updated_employee = Employee(
        employee_id=existing.employee_id,
        name=validation_helper.read_required_string("Enter New Name: "),
        email=read_valid_email(validation_helper),
        department=validation_helper.read_required_string("Enter New Department: "),
        designation=validation_helper.read_required_string("Enter New Designation: "),
        joining_date=validation_helper.read_valid_date("Enter New Joining Date (yyyy-MM-dd): "),
    )

    if service.update_employee(updated_employee):
        print("Employee updated successfully.")
    else:
        print("Employee could not be updated.")

    pause()


def delete_employee(service):
    print()
    employee_id = input("Enter Employee ID to delete: ").strip()
    if not employee_id:
        print("Employee ID cannot be empty.")
        pause()
        return

    confirmation = input("Are you sure you want to delete this employee? (y/n): ").strip().lower()
    if confirmation != "y":
        print("Deletion cancelled.")
        pause()
        return

    deleted = service.delete_employee(employee_id)
    if deleted:
        print("Employee deleted successfully.")
    else:
        print("Employee not found.")

    pause()


def sort_employees(service):
    print()
    employees = service.sort_employees_by_name()
    if not employees:
        print("No employees found.")
        pause()
        return

    for employee in employees:
        print(f"{employee.name} ({employee.employee_id})")

    pause()


def filter_employees(service):
    print()
    department = input("Enter Department: ").strip()
    if not department:
        print("Department cannot be empty.")
        pause()
        return

    employees = service.filter_by_department(department)
    if not employees:
        print("No employees found in this department.")
        pause()
        return

    for employee in employees:
        print(f"{employee.employee_id} - {employee.name} - {employee.designation}")

    pause()


def main():
    service = EmployeeService()
    validation_helper = ValidationHelper()

    while True:
        clear_screen()
        print("==============================")
        print("Employee Management System")
        print("==============================")
        print("1. Add Employee")
        print("2. View All Employees")
        print("3. Search Employee by ID")
        print("4. Update Employee")
        print("5. Delete Employee")
        print("6. Sort Employees by Name")
        print("7. Filter Employees by Department")
        print("8. Exit")
        print("==============================")
        choice = input("Enter your choice: ").strip()

        if choice == "1":
            add_employee(service, validation_helper)
        elif choice == "2":
            view_employees(service)
        elif choice == "3":
            search_employee(service)
        elif choice == "4":
            update_employee(service, validation_helper)
        elif choice == "5":
            delete_employee(service)
        elif choice == "6":
            sort_employees(service)
        elif choice == "7":
            filter_employees(service)
        elif choice == "8":
            return
        else:
            print("Invalid choice. Please try again.")
            pause()


if __name__ == "__main__":
    main()
