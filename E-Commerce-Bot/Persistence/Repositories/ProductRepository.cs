using E_Commerce_Bot.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _db.Products.Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Product> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
        public async Task<Product> GetByNameAsync(string text, string language)
        {
            return language switch
            {
                "uz" => await _db.Products.AsNoTracking().Include(x => x.Category).FirstOrDefaultAsync(x => x.Name_Uz == text),
                "ru" => await _db.Products.AsNoTracking().Include(x => x.Category).FirstOrDefaultAsync(x => x.Name_Ru == text),
                "en" => await _db.Products.AsNoTracking().Include(x => x.Category).FirstOrDefaultAsync(x => x.Name_En == text),
            };
        }
        public Task<bool> UpdateAsync(Product obj)
        {
            throw new NotImplementedException();
        }
    }
}
