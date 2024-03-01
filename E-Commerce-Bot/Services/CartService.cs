using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Bot.Services
{
    public class BasketService : IService<Basket>
    {
        private readonly ILogger<BasketService> _logger;
        private readonly ApplicationDbContext _db;

        public BasketService(ApplicationDbContext db, ILogger<BasketService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<bool> AddAsync(Basket newObject)
        {
            _db.Baskets.Add(newObject);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Basket = _db.Baskets.FirstOrDefault(x => x.Id == id);
            _db.Baskets.Remove(Basket);
            return await _db.SaveChangesAsync() > 0;
        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Basket>> GetAllAsync()
        {
            return _db.Baskets.AsNoTracking().ToListAsync();
        }

        public async Task<Basket> GetByIdAsync(int id)
        {
            return _db.Baskets.FirstOrDefault(x => x.Id == id);
        }

        public Task<Basket> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Basket> GetByNameAsync(string text)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Basket updatedobject)
        {
            _db.Baskets.Update(updatedobject);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
