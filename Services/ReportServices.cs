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
        Task<double> GetMonthlyRevenueAsync(int year, int month);
        Task<int> GetTotalPostsAsync();
    }
    public class ReportServices : IReportServices
    {
        private readonly UnitOfWork _unitOfWork;
        public ReportServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<double> GetMonthlyRevenueAsync(int year, int month)
        {
            return await _unitOfWork.BookingRepo.GetMonthlyRevenueAsync(year, month); 
        }

        public async Task<int> GetTotalCourtsAsync()
        {
            return await _unitOfWork.CourtRepo.CountAsync();    
        }

        public async Task<double> GetTotalRevenueAsync()
        {
            return await _unitOfWork.BookingRepo.SumAsync(p => p.TotalPrice ?? 0);
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
