namespace DIS_Dayforce_Assignment.Helpers
{
    public enum PayCode
    {
        Regular = 10,
        Overtime = 15,
        DoubleTime = 20
    }

    public class PayMultiplierHelper
    {
        public decimal GetPayMultiplier(string earningsCode)
        {
            // Use a switch expression to determine the multiplier
            return earningsCode switch
            {
                "Overtime" => (decimal)PayCode.Overtime/10, // Overtime multiplier
                "Double time" => (decimal)PayCode.DoubleTime/10, // Double-time multiplier
                _ => (decimal)PayCode.Regular/10  // Default to regular multiplier
            };
        }
    }
}
