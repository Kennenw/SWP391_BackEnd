using System;

namespace Repositories.Entities
{
    public partial class Payments
    {
        public int PaymentId { get; set; }
        public int? BookingId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double? PaymentAmount { get; set; }
        public double? TotalAmount { get; set; }
        public string PaymentCode { get; set; }
        public string Status { get; set; }
        public virtual Booking Booking { get; set; }
    }
}
