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
    public interface IPaymentMenthodServices
    {
        public List<PaymentMenthod> GetPaymentMenthod();
        public PaymentMenthod GetPaymentMenthodById(int id);
        public void CreatePaymentMenthod(PaymentMenthod paymentMenthod);
        public void UpdatePaymentMenthod(int id, PaymentMenthod paymentMenthod);
        public void DeletePaymentMenthod(int id);
    }
    public class PaymentMenthodServices : IPaymentMenthodServices
    {
        private readonly UnitOfWork _unitOfWork;

        public PaymentMenthodServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public void CreatePaymentMenthod(PaymentMenthod paymentMenthod)
        {
            paymentMenthod.Status = true;
            _unitOfWork.PaymentMenthodRepo.Create(paymentMenthod);
            _unitOfWork.SaveChanges();
        }

        public void DeletePaymentMenthod(int id)
        {
            var items = _unitOfWork.PaymentMenthodRepo.GetById(id);
            if(items != null)
            {
                items.Status = false;
                _unitOfWork.PaymentMenthodRepo.Update(items);
                _unitOfWork.SaveChanges();
            }
        }

        public List<PaymentMenthod> GetPaymentMenthod()
        {
            return _unitOfWork.PaymentMenthodRepo.GetAll(); 
        }

        public PaymentMenthod GetPaymentMenthodById(int id)
        {
            return _unitOfWork.PaymentMenthodRepo.GetById(id);
        }

        public void UpdatePaymentMenthod(int id, PaymentMenthod paymentMenthod)
        {
            _unitOfWork.PaymentMenthodRepo.Update(id, paymentMenthod);
            _unitOfWork.SaveChanges();
        }
    }
}
