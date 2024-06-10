using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using Services;

namespace BookingBad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentServices _vnPayService;
        private readonly IBookingSevices _bookingService;

        //public PaymentsController(IPaymentServices vnPayService, IBookingSevices bookingService)
        //{
        //    _vnPayService = vnPayService;
        //    _bookingService = bookingService;
        //}

        //// POST: api/Payments/CreatePayment
        //[HttpPost("CreatePayment")]
        //public ActionResult CreatePayment([FromBody] PaymentRequestDTO paymentRequest)
        //{
        //    try
        //    {
        //        string paymentUrl = _vnPayService.CreatePaymentUrl(paymentRequest, Request.Host.Value);
        //        return Ok(new { paymentUrl });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

    }
}
