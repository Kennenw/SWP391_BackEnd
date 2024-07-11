using Microsoft.AspNetCore.Mvc;
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
    private readonly IBookingSevices _bookingServices;
    private readonly IAccountServices _accountServices;
    private readonly GenericRepository<Payments> _paymentRepo;
    private readonly UnitOfWork _unitOfWork;

    public PaymentsController(IVNPayService vnPayService, IOptions<VnPayOption> options, IBookingSevices bookingServices,
        IAccountServices accountServices, GenericRepository<Payments> paymentRepo, UnitOfWork unitOfWork)
    {
        _vnPayService = vnPayService;
        _options = options;
        _bookingServices = bookingServices;
        _accountServices = accountServices;
        _paymentRepo = paymentRepo;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("create-payment")]
    public async Task<IActionResult> CreatePayment(int bookingId)
    {
        try
        {
            // Lấy thông tin booking từ BookingServices
            var booking = _bookingServices.GetBookingById(bookingId);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            var customerId = _bookingServices.GetCustomerIdByBookingId(bookingId);
            if (!customerId.HasValue)
            {
                return BadRequest("Customer ID not found for booking.");
            }
            var customerAccount = _accountServices.GetAccountById(customerId.Value);
            if (customerAccount == null)
            {
                return BadRequest("Customer account not found.");
            }
            if (customerAccount != null && booking.TotalPrice <= customerAccount.Balance)
            {
                var result = await _bookingServices.CompleteBookingWithoutBalance(bookingId);
                return CreatedAtAction(nameof(CreatePayment), new
                {
                    Message = "Booking completed successfully!",
                    Data = result
                });
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
                UserId = customerId,
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
            // Tạo URL thanh toán từ VNPayService
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

            // Lưu thông tin thanh toán vào cơ sở dữ liệu với trạng thái "pending"
            var payment = new Payments
            {
                BookingId = null,
                UserId = userId,
                PaymentDate = DateTime.Now,
                PaymentAmount = amount,
                TotalAmount = amount,
                PaymentCode = responseUriVnPay.Uri,
                Status = "pending"
            };
            _paymentRepo.Create(payment);

            return CreatedAtAction(nameof(CreateDeposit), new
            {
                Message = "Deposit URL created and payment saved successfully!",
                Data = responseUriVnPay
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update-payment-status/{paymentId}")]
    public IActionResult UpdatePaymentStatus(int paymentId)
    {
        try
        {
            var payment = _paymentRepo.GetById(paymentId);
            if (payment == null)
            {
                return NotFound("Payment not found.");
            }

            // Cập nhật trạng thái thanh toán thành công
            payment.Status = "success";

            _paymentRepo.Update(payment);

            // Lấy thông tin booking dựa trên BookingId của payment
            var booking = _bookingServices.GetBookingById((int)payment.BookingId);
            if (booking != null)
            {
                booking.Status = true;
                _unitOfWork.BookingRepo.UpdateBookingStatus((int)payment.BookingId, (bool)booking.Status);
                _unitOfWork.SaveChanges();
            }

            return Ok(new
            {
                Message = $"Updated payment status to 'success' for Payment ID {paymentId}.",
                Data = new { Payment = payment, Booking = booking }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }


    }
}
