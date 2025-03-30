namespace DIS_Dayforce_Assignment.DTO
{
    public class PaySummaryRecordDTO
    {
        public string EmployeeName { get; set; } // Name of the employee
        public string EmployeeNumber { get; set; } // Unique identifier for the employee } // Unique identifier for the employee
        public string EarningsCode { get; set; } // Code representing the type of earnings (e.g., regular, overtime)} // Code representing the type of earnings (e.g., regular, overtime)
        public decimal TotalHours { get; set; } // Total hours workedTotal hours worked
        public decimal TotalPayAmount { get; set; } // Total pay amount calculatedet; } // Total pay amount calculated
        public decimal RateOfPay { get; set; } // Rate of pay (hourly or adjusted) set; } // Rate of pay (hourly or adjusted)
        public string Job { get; set; } // Job title or role title or role
        public string Department { get; set; } // Department where the employee works   public string Department { get; set; } // Department where the employee works
    }
}