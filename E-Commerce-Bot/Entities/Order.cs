namespace E_Commerce_Bot.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Longitute { get; set; }
        public decimal Latitude { get; set; }
        public string? Address { get; set; }
        public double Price { get; set; }
        public long? UserId { get; set; }
        public User? User { get; set; }
        public int? Cart { get; set; }
        public Cart? Carts { get; set; }
    }
}
