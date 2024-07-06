namespace Repositories.DTO
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int? BookingId { get; set; }
        public int? UserId { get; set; } // Thêm dòng này để lưu thông tin UserId cho nạp tiền
        public DateTime? PaymentDate { get; set; }
        public double? PaymentAmount { get; set; }
        public double? TotalAmount { get; set; }
        public string PaymentCode { get; set; } // Thêm dòng này nếu cần lưu mã thanh toán
    }
}
