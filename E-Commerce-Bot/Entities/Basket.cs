namespace E_Commerce_Bot.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; } = null!;
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
