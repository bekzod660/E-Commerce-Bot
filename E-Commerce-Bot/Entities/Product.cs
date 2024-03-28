namespace E_Commerce_Bot.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name_Uz { get; set; }
        public string Name_Ru { get; set; }
        public string Name_En { get; set; }
        public string Description_Uz { get; set; }
        public string Description_Ru { get; set; }
        public string Description_En { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
