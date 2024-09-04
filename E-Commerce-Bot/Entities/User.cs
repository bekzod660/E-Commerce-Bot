using E_Commerce_Bot.Enums;

namespace E_Commerce_Bot.Entities
{
    public class User
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string Language { get; set; }
        public string? PhoneNumber { get; set; }
        public UserProcess UserProcess { get; set; }
        public string? Code { get; set; }
        public Basket Basket { get; set; } = null!;
        public int BasketId { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public ProcessHelper ProcessHelper { get; set; } = null!;
        public int ProcessHelperId { get; set; }

    }
}
