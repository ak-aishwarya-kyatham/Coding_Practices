import re
from datetime import datetime


class ValidationHelper:
    def is_null_or_white_space(self, value):
        return value is None or str(value).strip() == ""

    def is_valid_email(self, email):
        if self.is_null_or_white_space(email):
            return False

        normalized = email.strip()
        pattern = r"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"
        return bool(re.fullmatch(pattern, normalized)) and normalized == email

    def read_required_string(self, prompt):
        while True:
            value = input(prompt)
            if not self.is_null_or_white_space(value):
                return value.strip()
            print("Input cannot be empty.")

    def read_employee_id(self, prompt):
        while True:
            value = self.read_required_string(prompt)
            if len(value) >= 1:
                return value
            print("Employee ID cannot be empty.")

    def read_valid_date(self, prompt):
        while True:
            value = input(prompt)
            try:
                return datetime.strptime(value.strip(), "%Y-%m-%d").date()
            except ValueError:
                print("Please enter a valid date.")
