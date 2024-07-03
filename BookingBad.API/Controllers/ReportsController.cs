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
        public async Task<IActionResult> GetMonthlyRevenue(int year, int month, int day)
        {
            try
            {
                var revenue = await reportServices.GetRevenueAsync(year, month, day);
                return Ok(new { Year = year, Month = month, Day = day, Revenue = revenue });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("Book-Revenue")]
        public async Task<IActionResult> GetMonthlyRevenueBook(int year, int month, int day)
        {
            try
            {
                var revenue = await reportServices.GetRevenueTotalBooks(year, month, day);
                return Ok(new { Year = year, Month = month, Day = day, Revenue = revenue });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("Total-Post")]
        public async Task<IActionResult> GetTotalPost()
        {
            var postRevenue = await reportServices.GetTotalPostsAsync();
            return Ok(postRevenue);
        }
    }
}
