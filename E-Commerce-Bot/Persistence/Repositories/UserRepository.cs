using E_Commerce_Bot.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Bot.Persistence.Repositories
{
    public class UserRepository : IBaseRepository<User>
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddAsync(User obj)
        {
            _db.Users.Add(obj);
            return await _db.SaveChangesAsync() > 0;
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            User user = _db.Users.FirstOrDefault(x => x.Id == id);
            _db.Users.Remove(user);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return _db.Users.AsNoTracking().ToList();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(long id)
        {
            User user = _db.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        public Task<User> GetByNameAsync(string text)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _db.Users.Update(user);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
