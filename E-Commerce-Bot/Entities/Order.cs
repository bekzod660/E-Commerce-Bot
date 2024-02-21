namespace E_Commerce_Bot.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ItemId { get; set; }
        public virtual Item? Item { get; set; }
    }
}
