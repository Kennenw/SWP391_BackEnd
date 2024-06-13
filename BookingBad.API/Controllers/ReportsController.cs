using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace BookingBad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        public readonly IReportServices reportServices;
        public  ReportsController()
        {
            reportServices = new ReportServices();
        }
        [HttpGet("Total-Courts")]
        public async Task<IActionResult> GetTotalCourts()
        {
            var total = await reportServices.GetTotalCourtsAsync();
            return Ok(total);
        }

        [HttpGet("Total-Account")]
        public async Task<IActionResult> GetTotalAccount()
        {
            var total = await reportServices.GetTotalUsersAsync();
            return Ok(total);   
        }
        [HttpGet("Total-Revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var totalRevenue = await reportServices.GetTotalRevenueAsync();
            return Ok(totalRevenue);
        }

        [HttpGet("Monthly-Revenue")]
        public async Task<IActionResult> GetMonthlyRevenue(int year, int month)
        {
            var monthlyRevenue = await reportServices.GetMonthlyRevenueAsync(year, month);
            return Ok(monthlyRevenue);
        }

        [HttpGet("Total-Post")]
        public async Task<IActionResult> GetTotalPost()
        {
            var postRevenue = await reportServices.GetTotalPostsAsync();
            return Ok(postRevenue);
        }
    }
}
