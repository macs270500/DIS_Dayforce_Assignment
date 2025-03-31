using DIS_Dayforce_Assignment.DTO;

namespace DIS_Dayforce_Assignment.Services
{
    public interface IPayInfoService
    {
        Task<List<PaySummaryRecordDTO>> GetPayInfoAsync(List<(string EmployeeNumber, DateTime DateWorked)> employeeWorkInfos);
    }
}
