using E_Commerce_Bot.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Bot.Persistence.Repositories
{
    public class CategoryRepository : IBaseRepository<Category>
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public Task<bool> AddAsync(Category obj)
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

        public Task<Category> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Category obj)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _db.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category> GetByNameAsync(string text, string language)
        {
            return language switch
            {
                "uz" => await _db.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Name_Uz == text),
                "ru" => await _db.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Name_Ru == text),
                "en" => await _db.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Name_En == text),
            };
        }
    }
}
