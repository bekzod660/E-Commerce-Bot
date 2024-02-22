using E_Commerce_Bot.Entities;

namespace E_Commerce_Bot.Services
{
    public class CartService : IService<Cart>
    {
        public Task<bool> AddAsync(Cart newObject)
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

        public Task<List<Cart>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Cart> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Cart> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Cart updatedobject)
        {
            throw new NotImplementedException();
        }
    }
}
