namespace E_Commerce_Bot.Persistence.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> AddAsync(T obj);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(long id);
        Task<bool> UpdateAsync(T obj);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(long id);
        Task<List<T>> GetAllAsync();
    }
}
