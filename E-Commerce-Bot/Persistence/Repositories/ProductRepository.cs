using E_Commerce_Bot.Entities;

namespace E_Commerce_Bot.Persistence.Repositories
{
    public class ProductRepository : IBaseRepository<Product>
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddAsync(Product obj)
        {
            _db.Products.Add(obj);
            return await _db.SaveChangesAsync() > 0;
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

        public Task<bool> UpdateAsync(Product obj)
        {
            throw new NotImplementedException();
        }
    }
}
