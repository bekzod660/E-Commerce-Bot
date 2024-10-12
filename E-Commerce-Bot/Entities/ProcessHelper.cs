namespace E_Commerce_Bot.Entities
{
    public class ProcessHelper
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string? Comment { get; set; }
        public int OrderTypeId { get; set; }
        public string? PaymentType { get; set; }
        public long UserId { get; set; }
        public User? User { get; set; }
        public string? UserPhoneNumber { get; set; }
        public double Longitute { get; set; }
        public double Latitude { get; set; }
    }
}
