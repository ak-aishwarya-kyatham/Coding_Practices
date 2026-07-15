from dataclasses import dataclass
from datetime import date
from typing import Optional


@dataclass
class Employee:
    employee_id: str = ""
    name: str = ""
    email: str = ""
    department: str = ""
    designation: str = ""
    joining_date: Optional[date] = None

    @classmethod
    def from_dict(cls, data: dict) -> "Employee":
        joining_date_value = data.get("joining_date") or data.get("JoiningDate")
        joining_date = None
        if isinstance(joining_date_value, str):
            try:
                joining_date = date.fromisoformat(joining_date_value)
            except ValueError:
                joining_date = None

        return cls(
            employee_id=data.get("employee_id") or data.get("EmployeeId") or "",
            name=data.get("name") or data.get("Name") or "",
            email=data.get("email") or data.get("Email") or "",
            department=data.get("department") or data.get("Department") or "",
            designation=data.get("designation") or data.get("Designation") or "",
            joining_date=joining_date,
        )

    def to_dict(self) -> dict:
        return {
            "employee_id": self.employee_id,
            "name": self.name,
            "email": self.email,
            "department": self.department,
            "designation": self.designation,
            "joining_date": self.joining_date.isoformat() if self.joining_date else None,
        }
