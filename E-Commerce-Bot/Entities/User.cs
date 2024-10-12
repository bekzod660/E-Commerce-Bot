namespace E_Commerce_Bot.Entities
{
    public class User
    {
        public User(Basket basket, ProcessHelper processHelper, HashSet<Order> orders)
        {
            this.Basket = basket;
            this.ProcessHelper = processHelper;
            this.Orders = orders;
        }
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string Language { get; set; }
        public string? PhoneNumber { get; set; }
        public int UserStateId { get; set; }
        public string? Code { get; set; }
        public Basket Basket { get; set; }
        public int BasketId { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public ProcessHelper ProcessHelper { get; set; }
        public int ProcessHelperId { get; set; }

    }
}
