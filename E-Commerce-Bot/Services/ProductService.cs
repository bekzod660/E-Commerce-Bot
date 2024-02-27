using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Persistence;

namespace E_Commerce_Bot.Services
{
    public class ProductService : IService<Product>
    {
        private readonly ILogger<ProductService> _logger;
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db, ILogger<ProductService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<bool> AddAsync(Product newObject)
        {
            _db.Products.Add(newObject);
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

        public async Task<Product> GetByIdAsync(int id)
        {
            return _db.Products.FirstOrDefault(x => x.Id == id);
        }

        public Task<Product> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetByNameAsync(string text)
        {
            return _db.Products.FirstOrDefault(x => x.Name == text);
        }

        public Task<bool> UpdateAsync(Product updatedobject)
        {
            throw new NotImplementedException();
        }
    }
}
