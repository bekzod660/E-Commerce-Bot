namespace E_Commerce_Bot.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public virtual ICollection<Cart>? Carts { get; set; }
    }
}
