using DIS_Dayforce_Assignment.DTO;
using DIS_Dayforce_Assignment.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

namespace DIS_Dayforce_Assignment.Stores
{
    public class PayInfoStore : IPayInfoStore
    {
        private readonly IConfiguration _configuration;

        public PayInfoStore(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<List<PaySummaryRecordDTO>> SummarizePayInfoAsync(List<TimeCardRecordDTO> timeCard, List<RateTableRowDTO> rateTable)
        {
            var result = new List<PaySummaryRecordDTO>();
            var payMultiplierHelper = new PayMultiplierHelper();
            var jobAndDepartment = new EmployeeJobAndDepartmentDTO();

            foreach (var timecard in timeCard)
            {
                Decimal matchingRate;
                jobAndDepartment = await GetJobAndDepartmentAsync(timecard.EmployeeNumber, timecard.DateWorked);

                // With the following line to await the asynchronous method
                var hourlyRate = await GetHourlyRateAsync(jobAndDepartment);
                // Find the matching job rate
                if (timecard.EarningsCode != "Regular")
                {
                    matchingRate = await GetHourlyRateAsync(jobAndDepartment);
                }
                else
                {
                    matchingRate = rateTable.FirstOrDefault(rate =>
                       timecard.DateWorked >= rate.EffectiveStart &&
                       timecard.DateWorked <= rate.EffectiveEnd).HourlyRate;

                }
                // Determine the higher rate: base rate or job rate
                var applicableRate = timecard.Rate;
                if (matchingRate != null && matchingRate > timecard.Rate)
                {
                    applicableRate = matchingRate;
                }
                else if (matchingRate != null && hourlyRate > matchingRate && hourlyRate > timecard.Rate)
                {
                    applicableRate = hourlyRate;
                }

                    var finalRate = applicableRate * payMultiplierHelper.GetPayMultiplier(timecard.EarningsCode);
                var totalPay = finalRate * timecard.Hours + timecard.Bonus;

                var summary = result.FirstOrDefault(r => r.EmployeeNumber == timecard.EmployeeNumber);
                if (summary == null)
                {
                    summary = new PaySummaryRecordDTO
                    {
                        EmployeeName = timecard.EmployeeName,
                        EmployeeNumber = timecard.EmployeeNumber,
                        EarningsCode = timecard.EarningsCode,
                        TotalHours = timecard.Hours,
                        TotalPayAmount = Math.Round(totalPay, 2),
                        RateOfPay = finalRate,
                        Job = jobAndDepartment.Job,
                        Department = jobAndDepartment.Department.ToString()
                    };
                    result.Add(summary);
                }
                else
                {
                    summary.TotalHours += timecard.Hours;
                    summary.TotalPayAmount += totalPay;
                }
            }
            return result;
        }

        public async Task<List<RateTableRowDTO>> GetRateTableAsync()
        {
            var result = new List<RateTableRowDTO>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");
            using (var connection = new SqlConnection(connectionString))
            {
                var rateTableQuery = "SELECT * FROM rate_table";
                var rateTableCommand = new SqlCommand(rateTableQuery, connection);
                await connection.OpenAsync();
                using (var rateTableReader = await rateTableCommand.ExecuteReaderAsync())
                {
                    while (await rateTableReader.ReadAsync())
                    {
                        result.Add(new RateTableRowDTO
                        {
                            Job = rateTableReader["Job"].ToString(),
                            dept = rateTableReader["Dept"].ToString(),
                            EffectiveStart = ParseDate(rateTableReader["Effective_Start"].ToString()),
                            EffectiveEnd = ParseDate(rateTableReader["Effective_End"].ToString()),
                            HourlyRate = Convert.ToDecimal(rateTableReader["Hourly_Rate"])
                        });
                    }
                }
            }
            return result;
        }

        private async Task<EmployeeJobAndDepartmentDTO> GetJobAndDepartmentAsync(string employeeNumber, DateTime dateWorked)
        {
            EmployeeJobAndDepartmentDTO employeeJobAndDepartmentDTO = new EmployeeJobAndDepartmentDTO();
            var connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");
            using (var connection = new SqlConnection(connectionString))
            {
                var timeCardQuery = "SELECT * FROM TimecardFile WHERE EmployeeNumber = @EmployeeNumber AND DateWorked = @DateWorked";
                var timeCardCommand = new SqlCommand(timeCardQuery, connection);
                timeCardCommand.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);
                timeCardCommand.Parameters.AddWithValue("@DateWorked", ParseDate(dateWorked.ToString("yyyy-MM-dd")));
                await connection.OpenAsync();
                using (var timeCardReader = await timeCardCommand.ExecuteReaderAsync())
                {
                    while (await timeCardReader.ReadAsync())
                    {
                        employeeJobAndDepartmentDTO.Job = timeCardReader["JobWorked"].ToString();
                        employeeJobAndDepartmentDTO.Department = int.Parse(timeCardReader["DeptWorked"].ToString());
                        employeeJobAndDepartmentDTO.EmployeeNumber = employeeNumber;
                    }
                }
            }
            return employeeJobAndDepartmentDTO;
        }

        public async Task<decimal> GetHourlyRateAsync(EmployeeJobAndDepartmentDTO employeeJobAndDepartmentDTO)
        {
            if (employeeJobAndDepartmentDTO == null)
            {
                throw new ArgumentNullException(nameof(employeeJobAndDepartmentDTO));
            }

            if (string.IsNullOrEmpty(employeeJobAndDepartmentDTO.Job) || employeeJobAndDepartmentDTO.Department == null)
            {
                throw new ArgumentException("Job and Department cannot be null or empty.");
            }

            decimal hourlyRate = 0;
            var connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");

            using (var connection = new SqlConnection(connectionString))
            {
                var rateTableQuery = "SELECT Hourly_Rate FROM rate_table WHERE Job=@Job AND Dept=@Dept";
                var rateTableCommand = new SqlCommand(rateTableQuery, connection);
                rateTableCommand.Parameters.AddWithValue("@Job", employeeJobAndDepartmentDTO.Job);
                rateTableCommand.Parameters.AddWithValue("@Dept", employeeJobAndDepartmentDTO.Department);

                await connection.OpenAsync();
                using (var rateTableReader = await rateTableCommand.ExecuteReaderAsync())
                {
                    if (await rateTableReader.ReadAsync())
                    {
                        hourlyRate = Convert.ToDecimal(rateTableReader["Hourly_Rate"]);
                    }
                    else
                    {
                        // Log or handle the case where no matching record is found
                        Console.WriteLine($"No matching rate found for Job: {employeeJobAndDepartmentDTO.Job}, Dept: {employeeJobAndDepartmentDTO.Department}");
                    }
                }
            }

            if (hourlyRate == 0)
            {
                throw new InvalidOperationException($"No hourly rate found for Job: {employeeJobAndDepartmentDTO.Job}, Dept: {employeeJobAndDepartmentDTO.Department}");
            }

            return hourlyRate;
        }

        public async Task<List<TimeCardRecordDTO>> GetTimeCardRecordsAsync(string employeeNumber, DateTime dateWorked)
        {
            var result = new List<TimeCardRecordDTO>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");
            using (var connection = new SqlConnection(connectionString))
            {
                var timeCardQuery = "SELECT * FROM TimecardFile WHERE EmployeeNumber = @EmployeeNumber AND DateWorked = @DateWorked";
                var timeCardCommand = new SqlCommand(timeCardQuery, connection);
                timeCardCommand.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);
                timeCardCommand.Parameters.AddWithValue("@DateWorked", ParseDate(dateWorked.ToString("yyyy-MM-dd")));
                await connection.OpenAsync();
                using (var timeCardReader = await timeCardCommand.ExecuteReaderAsync())
                {
                    while (await timeCardReader.ReadAsync())
                    {
                        result.Add(new TimeCardRecordDTO
                        {
                            EmployeeName = timeCardReader["EmployeeName"].ToString(),
                            EmployeeNumber = timeCardReader["EmployeeNumber"].ToString(),
                            DateWorked = ParseDate(timeCardReader["DateWorked"].ToString()),
                            EarningsCode = timeCardReader["EarningsCode"].ToString(),
                            Hours = Convert.ToDecimal(timeCardReader["Hours"]),
                            Rate = Convert.ToDecimal(timeCardReader["Rate"]),
                            Bonus = Convert.ToDecimal(timeCardReader["Bonus"])
                        });
                    }
                }
            }
            return result;
        }

        private DateTime ParseDate(string dateString)
        {
            // Try to parse the input string into a DateTime object
            if (DateTime.TryParse(dateString, null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }
            throw new FormatException($"Unable to parse date: {dateString}");
        }
    }

}
