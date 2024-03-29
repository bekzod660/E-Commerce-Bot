namespace E_Commerce_Bot.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public string? PhoneNumber { get; set; }
        public UserProcess UserProcess { get; set; }
        public Basket Basket { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public ProcessHelper ProcessHelper { get; set; } = null!;

    }
}
