import json
import os
from typing import List, Optional

from models import Employee


class EmployeeService:
    def __init__(self, storage_path: Optional[str] = None):
        self._employees: List[Employee] = []
        self._storage_path = storage_path or os.path.join(os.path.dirname(__file__), "employees.json")
        self.load_employees()

    def add_employee(self, employee: Optional[Employee]) -> bool:
        if employee is None:
            return False

        if any(e.employee_id.lower() == employee.employee_id.lower() for e in self._employees):
            return False

        self._employees.append(employee)
        self.save_employees()
        return True

    def view_employees(self) -> List[Employee]:
        return sorted(self._employees, key=lambda e: e.name.lower())

    def search_employee_by_id(self, employee_id: str) -> Optional[Employee]:
        if self.is_null_or_white_space(employee_id):
            return None

        for employee in self._employees:
            if employee.employee_id.lower() == employee_id.strip().lower():
                return employee
        return None

    def update_employee(self, updated_employee: Employee) -> bool:
        existing = self.search_employee_by_id(updated_employee.employee_id)
        if existing is None:
            return False

        existing.name = updated_employee.name
        existing.email = updated_employee.email
        existing.department = updated_employee.department
        existing.designation = updated_employee.designation
        existing.joining_date = updated_employee.joining_date
        self.save_employees()
        return True

    def delete_employee(self, employee_id: str) -> bool:
        employee = self.search_employee_by_id(employee_id)
        if employee is None:
            return False

        self._employees.remove(employee)
        self.save_employees()
        return True

    def sort_employees_by_name(self) -> List[Employee]:
        return sorted(self._employees, key=lambda e: e.name.lower())

    def filter_by_department(self, department: str) -> List[Employee]:
        return sorted(
            [e for e in self._employees if e.department.lower() == department.strip().lower()],
            key=lambda e: e.name.lower(),
        )

    def load_employees(self) -> None:
        if not os.path.exists(self._storage_path):
            return

        try:
            with open(self._storage_path, "r", encoding="utf-8") as handle:
                payload = handle.read().strip()

            if not payload:
                self._employees = []
                return

            raw_items = json.loads(payload)
            self._employees = [Employee.from_dict(item) for item in raw_items]
        except Exception:
            self._employees = []

    def save_employees(self) -> None:
        try:
            directory = os.path.dirname(self._storage_path)
            if directory and not os.path.exists(directory):
                os.makedirs(directory, exist_ok=True)

            with open(self._storage_path, "w", encoding="utf-8") as handle:
                json.dump([employee.to_dict() for employee in self._employees], handle, indent=2)
        except Exception:
            pass

    @staticmethod
    def is_null_or_white_space(value) -> bool:
        return value is None or str(value).strip() == ""
