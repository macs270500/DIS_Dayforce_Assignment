namespace DIS_Dayforce_Assignment.DTO
{
    public class RateTableRowDTO
    {
        public string Job { get; set; } // Job title or role
        public string dept { get; set; } // Department name
        public DateTime EffectiveStart { get; set; } // Start date of the rate's validity
        public DateTime EffectiveEnd { get; set; } // End date of the rate's validity
        public decimal HourlyRate { get; set; } // Hourly rate for the job and department
    }
}
