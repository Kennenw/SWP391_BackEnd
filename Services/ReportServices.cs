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
        Task<int> GetTotalBookingsCountAsync();
        Task<double> GetSuccessfulBookingRateAsync();
        Task<List<object>> GetTotalRevenueByMonthYearAsync();
        int GetTotalCourtsCount();
        int GetTotalAccountsCount();
        Task<double> GetTotalRevenueAsync();
        int GetTotalPostsCount();
    }
    public class ReportServices : IReportServices
    {
        private readonly UnitOfWork _unitOfWork;
        public ReportServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<int> GetTotalBookingsCountAsync()
        {
            return _unitOfWork.BookingRepo.GetTotalBookingsCount();
        }

        public async Task<double> GetSuccessfulBookingRateAsync()
        {
            return await _unitOfWork.BookingRepo.GetSuccessfulBookingRateAsync();
        }

        public async Task<double> GetTotalRevenueAsync()
        {
            return _unitOfWork.PaymentRepo.GetTotalRevenue();
        }

        public int GetTotalCourtsCount()
        {
            return _unitOfWork.CourtRepo.GetTotalCourtsCount();
        }

        public int GetTotalAccountsCount()
        {
            return _unitOfWork.AccountRepo.GetTotalAccountsCount();
        }

        public int GetTotalPostsCount()
        {
            return _unitOfWork.PostRepo.GetTotalPosts();
        }
        public async Task<List<object>> GetTotalRevenueByMonthYearAsync()
        {
            return await _unitOfWork.PaymentRepo.GetTotalRevenueByMonthYear();
        }
    }
}
