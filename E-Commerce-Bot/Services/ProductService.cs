using E_Commerce_Bot.Entities;

namespace E_Commerce_Bot.Services
{
    public class ProductService : IService<Product>
    {
        public Task<bool> AddAsync(Product newObject)
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

        public Task<List<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Product updatedobject)
        {
            throw new NotImplementedException();
        }
    }
}
