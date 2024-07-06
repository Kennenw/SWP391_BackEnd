using Microsoft.AspNetCore.Http;
using Repositories.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IVNPayService
    {
        ResponseUriModel CreatePayment(PaymentInfoModel model, HttpContext context);
        ResponseUriModel CreateDeposit(PaymentInfoModel model, HttpContext context, int userId); 
        PaymentResponseModel PaymentExecute(IQueryCollection collection);
    }
}
