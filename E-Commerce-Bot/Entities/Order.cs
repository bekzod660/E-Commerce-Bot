namespace E_Commerce_Bot.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public double Longitute { get; set; }
        public double Latitude { get; set; }
        public string? Address { get; set; }
        public string? Comment { get; set; }
        public double TotalPrice
        {
            get
            {
                if (Products != null)
                    return Products.Select(p => p.Price).Sum();
                else
                    return 0;
            }
        }

        public OrderType OrderType { get; set; }
        public string PaymentType { get; set; }
        public bool IsDelivered { get; set; } = false;
        public bool IsPaid { get; set; } = false;
        public long UserId { get; set; }
        public User? User { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
