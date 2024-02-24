using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Bot.Services
{
    public class UserService : IService<User>
    {
        private readonly ILogger<UserService> _logger;
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext db, ILogger<UserService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<bool> AddAsync(User newObject)
        {
            _db.Users.Add(newObject);
            int result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            User user = _db.Users.FirstOrDefault(x => x.Id == id);
            _db.Users.Remove(user);
            int result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public Task<List<User>> GetAllAsync()
        {
            return _db.Users.ToListAsync();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(long id)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<User> GetByName(string text)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(User updatedobject)
        {
            _db.Users.Update(updatedobject);
            int result = await _db.SaveChangesAsync();
            return result > 0;
        }

        internal Task AddAsync(Telegram.Bot.Types.User user)
        {
            throw new NotImplementedException();
        }
    }
}
