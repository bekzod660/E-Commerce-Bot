namespace E_Commerce_Bot.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public virtual ICollection<Item>? Items { get; set; }
    }
}
