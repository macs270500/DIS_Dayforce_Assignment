using DIS_Dayforce_Assignment.DTO;

namespace DIS_Dayforce_Assignment.Services
{
    public interface IPayInfoService
    {
        List<PaySummaryRecordDTO> GetPayInfo(List<(string EmployeeNumber, DateTime DateWorked)> employeeWorkInfos);
    }
}
