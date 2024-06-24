using Microsoft.AspNetCore.Mvc;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingBad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentServices _paymentService;

        public PaymentsController(IPaymentServices paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment(int bookingId)
        {
            try
            {
                var paymentUrl = await _paymentService.CreatePaymentUrl(bookingId);
                return Ok(new { paymentUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("return")]
        public IActionResult PaymentReturn([FromQuery] Dictionary<string, string> queryParams)
        {
            if (queryParams == null || !queryParams.Any())
                return BadRequest("Invalid parameters");

            var signature = queryParams["vnp_SecureHash"];
            queryParams.Remove("vnp_SecureHash");

            bool isValid = _paymentService.ValidateSignature(queryParams, signature);
            if (!isValid)
            {
                return BadRequest("Invalid signature");
            }

            // Handle the return logic here
            return Ok("Payment successful");
        }

        [HttpPost("notify")]
        public IActionResult PaymentNotify([FromForm] Dictionary<string, string> queryParams)
        {
            if (queryParams == null || !queryParams.Any())
                return BadRequest("Invalid parameters");

            var signature = queryParams["vnp_SecureHash"];
            queryParams.Remove("vnp_SecureHash");

            bool isValid = _paymentService.ValidateSignature(queryParams, signature);
            if (!isValid)
            {
                return BadRequest("Invalid signature");
            }

            // Handle the notification logic here
            return Ok("Notification received");
        }
    }
}
