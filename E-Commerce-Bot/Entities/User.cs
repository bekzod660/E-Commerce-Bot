namespace E_Commerce_Bot.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public Process UserProcess { get; set; }
        public int CartId { get; set; }
        public Cart? Cart { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
