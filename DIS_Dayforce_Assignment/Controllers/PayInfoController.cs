using DIS_Dayforce_Assignment.DTO;
using DIS_Dayforce_Assignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace DIS_Dayforce_Assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayInfoController : ControllerBase
    {
        private readonly IPayInfoService _payInfoService;

        public PayInfoController(IPayInfoService payInfoService)
        {
            _payInfoService = payInfoService;
        }

        [HttpGet("GetEmployeeData")]
        public async Task<ActionResult<List<PaySummaryRecordDTO>>> GetPayInfo([FromQuery] List<string> employeeNumbers, [FromQuery] List<DateTime> datesWorked)
        {
            if (employeeNumbers == null || datesWorked == null || employeeNumbers.Count != datesWorked.Count)
            {
                return BadRequest("EmployeeNumbers and DatesWorked are required and must have the same length.");
            }

            var employeeWorkInfos = employeeNumbers.Zip(datesWorked, (number, date) => (number, date)).ToList();

            var payInfos = await _payInfoService.GetPayInfoAsync(employeeWorkInfos);
            if (payInfos == null || !payInfos.Any())
            {
                return NotFound();
            }
            return Ok(payInfos);
        }
    }
}
