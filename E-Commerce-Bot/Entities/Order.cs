namespace E_Commerce_Bot.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public double Longitute { get; set; }
        public double Latitude { get; set; }
        public string? Address { get; set; }
        public double Price { get; set; }
        public OrderType OrderType { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsDelivered { get; set; } = false;
        public long? UserId { get; set; }
        public User? User { get; set; }
    }
}
