using E_Commerce_Bot.Entities;

namespace E_Commerce_Bot.Persistence.Repositories
{
    public class CategoryRepository : IBaseRepository<Category>
    {
        public Task<bool> AddAsync(Category obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetAllAsync()
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

        public Task<bool> UpdateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
