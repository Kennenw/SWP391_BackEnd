using BookingBad.DAL;
using BookingBad.BLL.DTO;
using BookingBad.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.BLL.Services
{
    public interface IPaymentServices
    {
        public List<Payment> GetPayment();
        public Payment GetPaymentById(int id);
        public void CreatePayment(Payment payment);
        public void UpdatePayment(int id, Payment payment);
        public void DeletePayment(int id);
    }
    public class PaymentServices : IPaymentServices
    {
        private readonly UnitOfWork _unitOfWork;

        public PaymentServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public void CreatePayment(Payment payment)
        {
            payment.Status = true;
            _unitOfWork.PaymentRepo.Create(payment);
            _unitOfWork.SaveChanges();
        }

        public void DeletePayment(int id)
        {
            var items = _unitOfWork.PaymentRepo.GetById(id);
            if(items != null)
            {
                items.Status = false;
                _unitOfWork.PaymentRepo.Update(items);
                _unitOfWork.SaveChanges();
            }
        }

        public List<Payment> GetPayment()
        {
            return _unitOfWork.PaymentRepo.GetAll();
        }

        public Payment GetPaymentById(int id)
        {
            return _unitOfWork.PaymentRepo.GetById(id);
        }

        public void UpdatePayment(int id, Payment payment)
        {
            _unitOfWork.PaymentRepo.Update(id, payment);
            _unitOfWork.SaveChanges();
        }
    }
}
