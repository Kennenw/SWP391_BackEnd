using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.BLL.DTO
{
    public class SortContent
    {
        public SortCourtByEnum sortCourtBy { get; set; }
        public SortAccountByEnum sortAccountBy { get; set; }
        public SortBookingByEnum sortBookingBy { get; set; }
        public SortTypeEnum sortType { get; set; }
    }

    public enum SortCourtByEnum
    {
        CourtId = 1,
        CourtName = 2,
        AreaId = 3,
        OpenTime = 4,
    }

    public enum SortBookingByEnum
    {
        BookingId = 1,
        CustomerId = 2,
        BookingTypeId = 3,
        TotalPrice = 4,
    }

    public enum SortAccountByEnum
    {
        AccountId = 1,
        AccountName = 2,
        FullName = 3,
        RoleId = 4,
    }


    public enum SortTypeEnum
    {
        Ascending = 1,
        Descending = 2,
    }
}
