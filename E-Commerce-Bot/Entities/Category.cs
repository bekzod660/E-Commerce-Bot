namespace E_Commerce_Bot.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name_Uz { get; set; }
        public string Name_Ru { get; set; }
        public string Name_En { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
