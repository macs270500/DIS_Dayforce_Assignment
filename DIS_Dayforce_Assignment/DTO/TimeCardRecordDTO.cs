namespace DIS_Dayforce_Assignment.DTO
{
    public class TimeCardRecordDTO
    {
        public string EmployeeName { get; set; } // Name of the employee
        public string EmployeeNumber { get; set; } // Unique identifier for the employee
        public DateTime DateWorked { get; set; } // Date the employee worked
        public string EarningsCode { get; set; } // Code representing the type of earnings (e.g., regular, overtime)
        public decimal Hours { get; set; } // Number of hours worked
        public decimal Rate { get; set; } // Base hourly rate for the employee
        public decimal Bonus { get; set; } // Bonus amount for the employee
    }
}
