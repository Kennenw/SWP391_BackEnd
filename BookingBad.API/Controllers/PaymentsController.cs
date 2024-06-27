using Microsoft.AspNetCore.Mvc;
using Repositories.Payment;
using Repositories;
using Microsoft.Extensions.Options;
using Services;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IVNPayService _vnPayService;
    private readonly IOptions<VnPayOption> _options;
    private readonly IBookingSevices _bookingSevices;


    public PaymentsController(IVNPayService vnPayService, IOptions<VnPayOption> options, IBookingSevices bookingServices)
    {
        _vnPayService = vnPayService;
        _options = options;
        _bookingSevices = bookingServices;
    }

    /*    [HttpPost("create-payment")]
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
        }*/

    [HttpPost("create-payment")]
    public async Task<IActionResult> CreatePayment(int bookingId)
    {
        try
        {
            // Lấy thông tin booking từ BookingServices
            var booking = _bookingSevices.GetBookingById(bookingId);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            // Tạo URL thanh toán từ VNPayService
            var responseUriVnPay = _vnPayService.CreatePayment(new PaymentInfoModel()
            {
                TotalAmount = (double)booking.TotalPrice,
                PaymentCode = booking.BookingId + "." + Guid.NewGuid()
            }, HttpContext);

            if (string.IsNullOrEmpty(responseUriVnPay.Uri))
            {
                return new BadRequestObjectResult(new
                {
                    Message = "Không thể tạo url thanh toán vào lúc này !"
                });
            }

            return Ok(new SuccessObject<object>
            {
                Message = "Tạo url thành công!",
                Data = responseUriVnPay
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

/*    [HttpGet("return")]
    public IActionResult PaymentReturn([FromQuery] Dictionary<string, string> queryParams)
    {
        if (queryParams == null || !queryParams.Any())
            return BadRequest("Invalid parameters");

        if (!queryParams.TryGetValue("vnp_SecureHash", out var signature))
            return BadRequest("Missing vnp_SecureHash parameter");

        queryParams.Remove("vnp_SecureHash");

        bool isValid = _paymentService.ValidateSignature(queryParams, signature);

        if (isValid)
        {
            // Handle successful payment return logic here
            return Ok("Payment successful");
        }
        else
        {
            return BadRequest("Invalid signature");
        }
    }*/

/*    [HttpPost("notify")]
    public IActionResult PaymentNotify([FromForm] Dictionary<string, string> queryParams)
    {
        if (queryParams == null || !queryParams.Any())
            return BadRequest("Invalid parameters");

        if (!queryParams.TryGetValue("vnp_SecureHash", out var signature))
            return BadRequest("Missing vnp_SecureHash parameter");

        queryParams.Remove("vnp_SecureHash");

        bool isValid = _paymentService.ValidateSignature(queryParams, signature);
        if (isValid)
        {
            // Handle notification logic here
            return Ok("Notification received");
        }
        else
        {
            return BadRequest("Invalid signature");
        }
    }*/
}
