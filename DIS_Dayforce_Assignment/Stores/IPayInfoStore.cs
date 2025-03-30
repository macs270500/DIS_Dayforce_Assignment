using DIS_Dayforce_Assignment.DTO;

namespace DIS_Dayforce_Assignment.Stores
{
    public interface IPayInfoStore
    {
        List<PaySummaryRecordDTO> SummarizePayInfo(List<TimeCardRecordDTO> timeCard, List<RateTableRowDTO> rateTable);
        List<TimeCardRecordDTO> GetTimeCardRecords(string employeeNumber, DateTime dateWorked);
        List<RateTableRowDTO> GetRateTable();
    }
}