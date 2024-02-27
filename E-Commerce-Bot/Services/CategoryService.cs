using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Bot.Services
{
    public class CategoryService : IService<Category>
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly ApplicationDbContext _db;

        public CategoryService(ApplicationDbContext db, ILogger<CategoryService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<bool> AddAsync(Category newObject)
        {
            _db.Categories.Add(newObject);
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

        public Task<List<Category>> GetAllAsync()
        {
            return _db.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _db.Categories.Include(x => x.Products)
                .FirstOrDefaultAsync(y => y.Id == id);
        }

        public Task<Category> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> GetByNameAsync(string text)
        {
            return _db.Categories.Include(x => x.Products).FirstOrDefault(x => x.Name == text);
        }

        public Task<bool> UpdateAsync(Category updatedobject)
        {
            throw new NotImplementedException();
        }
    }
}
