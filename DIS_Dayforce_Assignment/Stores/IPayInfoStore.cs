using DIS_Dayforce_Assignment.DTO;

namespace DIS_Dayforce_Assignment.Stores
{
    public interface IPayInfoStore
    {
        Task<List<PaySummaryRecordDTO>> SummarizePayInfoAsync(List<TimeCardRecordDTO> timeCard, List<RateTableRowDTO> rateTable);
        Task<List<TimeCardRecordDTO>> GetTimeCardRecordsAsync(string employeeNumber, DateTime dateWorked);
        Task<List<RateTableRowDTO>> GetRateTableAsync();
    }
}