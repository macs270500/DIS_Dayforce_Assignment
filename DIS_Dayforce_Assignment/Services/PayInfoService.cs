using DIS_Dayforce_Assignment.DTO;
using DIS_Dayforce_Assignment.Stores;

namespace DIS_Dayforce_Assignment.Services
{
    public class PayInfoService : IPayInfoService
    {
        private readonly IPayInfoStore _payInfoStore;

        public PayInfoService(IPayInfoStore payInfoStore)
        {
            _payInfoStore = payInfoStore ?? throw new ArgumentNullException(nameof(payInfoStore));
        }

        public async Task<PaySummaryRecordDTO> GetPayInfoByEmployeeNumberAndDateAsync(string employeeNumber, DateTime dateWorked)
        {
            var paySummary = new PaySummaryRecordDTO();
            var timeCardRecords = await _payInfoStore.GetTimeCardRecordsAsync(employeeNumber, dateWorked);
            var rateTableRecords = await _payInfoStore.GetRateTableAsync();

            // Summarize Pay Info
            var summarizedPayInfo = await _payInfoStore.SummarizePayInfoAsync(timeCardRecords, rateTableRecords);

            // Get the specific pay summary for the given employee number and date worked
            paySummary = summarizedPayInfo.FirstOrDefault(p => p.EmployeeNumber == employeeNumber && p.TotalHours > 0);

            return paySummary;
        }

        public async Task<List<PaySummaryRecordDTO>> GetPayInfoAsync(List<(string EmployeeNumber, DateTime DateWorked)> employeeWorkInfos)
        {
            var paySummaries = new List<PaySummaryRecordDTO>();

            foreach (var (employeeNumber, dateWorked) in employeeWorkInfos)
            {
                var paySummary = await GetPayInfoByEmployeeNumberAndDateAsync(employeeNumber, dateWorked);
                if (paySummary != null)
                {
                    paySummaries.Add(paySummary);
                }
            }

            return paySummaries;
        }
    }
    
}
