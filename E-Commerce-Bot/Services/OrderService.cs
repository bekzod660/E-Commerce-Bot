using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Persistence;

namespace E_Commerce_Bot.Services
{
    public class OrderService : IService<Order>
    {
        private readonly ILogger<OrderService> _logger;
        private readonly ApplicationDbContext _db;

        public OrderService(ApplicationDbContext db, ILogger<OrderService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public Task<bool> AddAsync(Order newObject)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetByName(string text)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Order updatedobject)
        {
            throw new NotImplementedException();
        }
    }
}
