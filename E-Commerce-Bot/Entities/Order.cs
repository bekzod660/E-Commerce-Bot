namespace E_Commerce_Bot.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Longitute { get; set; }
        public decimal Latitude { get; set; }
        public string? Address { get; set; }
        public double Price { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ItemId { get; set; }

        public virtual ICollection<Item>? Item { get; set; }
    }
}
