using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IReportServices
    {
        Task<int> GetTotalCourtsAsync();
        Task<int> GetTotalUsersAsync();
        Task<double> GetTotalRevenueAsync();
        Task<double> GetRevenueAsync(int year, int month, int? day);
        Task<int> GetTotalPostsAsync();
        Task<double> GetRevenueTotalBooks(int year, int month, int? day);
    }
    public class ReportServices : IReportServices
    {
        private readonly UnitOfWork _unitOfWork;
        public ReportServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<double> GetRevenueAsync(int year, int month, int? day)
        {
            return await _unitOfWork.PaymentRepo.GetRevenueAsync(year, month, day); 
        }

        public async Task<double> GetRevenueTotalBooks(int year, int month, int? day)
        {
            return await _unitOfWork.BookingRepo.GetRevenueTotalBook(year, month, day);
        }

        public async Task<int> GetTotalCourtsAsync()
        {
            return await _unitOfWork.CourtRepo.CountAsync();    
        }

        public async Task<double> GetTotalRevenueAsync()
        {
            return await _unitOfWork.PaymentRepo.SumAsync(p => p.TotalAmount ?? 0);
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _unitOfWork.AccountRepo.CountAsync();
        }

        public async Task<int> GetTotalPostsAsync()
        {
            return await _unitOfWork.PostRepo.CountAsync();
        }
    }
}
