using E_Commerce_Bot.Entities;

namespace E_Commerce_Bot.Services
{
    public class OrderService : IService<Order>
    {
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

        public Task<bool> UpdateAsync(Order updatedobject)
        {
            throw new NotImplementedException();
        }
    }
}
