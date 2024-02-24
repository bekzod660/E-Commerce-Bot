using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Bot.Services
{
    public class CartService : IService<Cart>
    {
        private readonly ILogger<CartService> _logger;
        private readonly ApplicationDbContext _db;

        public CartService(ApplicationDbContext db, ILogger<CartService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<bool> AddAsync(Cart newObject)
        {
            _db.Carts.Add(newObject);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cart = _db.Carts.FirstOrDefault(x => x.Id == id);
            _db.Carts.Remove(cart);
            return await _db.SaveChangesAsync() > 0;
        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Cart>> GetAllAsync()
        {
            return _db.Carts.AsNoTracking().ToListAsync();
        }

        public async Task<Cart> GetByIdAsync(int id)
        {
            return _db.Carts.FirstOrDefault(x => x.Id == id);
        }

        public Task<Cart> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Cart> GetByName(string text)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Cart updatedobject)
        {
            _db.Carts.Update(updatedobject);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
