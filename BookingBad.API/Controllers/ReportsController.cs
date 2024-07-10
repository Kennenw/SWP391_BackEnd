using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities;
using Services;

namespace BookingBad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        public readonly IReportServices _reportServices;
        public ReportsController(ReportServices reportServices)
        {
            _reportServices = reportServices;
        }

        [HttpGet("total-bookings")]
        public async Task<IActionResult> GetTotalBookingsCount()
        {
            var count = await _reportServices.GetTotalBookingsCountAsync();
            return Ok(count);
        }


        [HttpGet("successful-booking-rate")]
        public async Task<IActionResult> GetSuccessfulBookingRate()
        {
            var rate = await _reportServices.GetSuccessfulBookingRateAsync();
            return Ok(rate);
        }

        [HttpGet("total-courts")]
        public IActionResult GetTotalCourtsCount()
        {
            var count = _reportServices.GetTotalCourtsCount();
            return Ok(count);
        }

        [HttpGet("total-accounts")]
        public IActionResult GetTotalAccountsCount()
        {
            var count = _reportServices.GetTotalAccountsCount();
            return Ok(count);
        }

        [HttpGet("total-revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var revenue = await _reportServices.GetTotalRevenueAsync();
            return Ok(revenue);
        }

        [HttpGet("total-posts")]
        public IActionResult GetTotalPostsCount()
        {
            var count = _reportServices.GetTotalPostsCount();
            return Ok(count);
        }
        [HttpGet("total-revenue-by-month-year")]
        public async Task<IActionResult> GetTotalRevenueByMonthYear()
        {
            var revenueByMonthYear = await _reportServices.GetTotalRevenueByMonthYearAsync();
            return Ok(revenueByMonthYear);
        }
    }
}
