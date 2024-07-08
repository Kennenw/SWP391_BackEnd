﻿using Microsoft.AspNetCore.Mvc;
using Repositories.Payment;
using Repositories;
using Microsoft.Extensions.Options;
using Services;
using Repositories.Entities;
using Repositories.Repositories;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IVNPayService _vnPayService;
    private readonly IOptions<VnPayOption> _options;
    private readonly IBookingSevices _bookingSevices;
    private readonly IAccountServices _accountServices;
    private readonly GenericRepository<Payments> _paymentRepo;


    public PaymentsController(IVNPayService vnPayService, IOptions<VnPayOption> options, IBookingSevices bookingServices, IAccountServices accountServices, GenericRepository<Payments> paymentRepo)
    {
        _vnPayService = vnPayService;
        _options = options;
        _bookingSevices = bookingServices;
        _accountServices = accountServices;
        _paymentRepo = paymentRepo;
    }


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

            if (responseUriVnPay == null || string.IsNullOrEmpty(responseUriVnPay.Uri))
            {
                return new BadRequestObjectResult(new
                {
                    Message = "Không thể tạo url thanh toán vào lúc này!"
                });
            }

            // Lưu thông tin thanh toán vào cơ sở dữ liệu với trạng thái "pending"
            var payment = new Payments
            {
                BookingId = bookingId,
                PaymentDate = DateTime.Now,
                PaymentAmount = booking.TotalPrice,
                TotalAmount = booking.TotalPrice,
                PaymentCode = responseUriVnPay.Uri,
                Status = "pending"
            };
            _paymentRepo.Create(payment);

            return CreatedAtAction(nameof(CreatePayment), new
            {
                Message = "Tạo url thành công và đã lưu thanh toán!",
                Data = responseUriVnPay
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("create-deposit")]
    public async Task<IActionResult> CreateDeposit(int userId, double amount)
    {
        try
        {
            var responseUriVnPay = _vnPayService.CreateDeposit(new PaymentInfoModel()
            {
                TotalAmount = amount,
                PaymentCode = userId + "." + Guid.NewGuid()
            }, HttpContext, userId);

            if (responseUriVnPay == null || string.IsNullOrEmpty(responseUriVnPay.Uri))
            {
                return new BadRequestObjectResult(new
                {
                    Message = "Cannot create deposit URL at this moment!"
                });
            }

            return Ok(new
            {
                Message = "Deposit URL created successfully!",
                Data = responseUriVnPay
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
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
}
