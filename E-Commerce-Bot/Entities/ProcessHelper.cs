namespace E_Commerce_Bot.Entities
{
    public class ProcessHelper
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public long? UserId { get; set; }
        public OrderType? Order { get; set; }
        public User? User { get; set; }
        public double? Longitute { get; set; }
        public double? Latitude { get; set; }
    }
}
