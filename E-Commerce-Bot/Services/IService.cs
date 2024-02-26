namespace E_Commerce_Bot.Services
{
    public interface IService<T> where T : class
    {
        Task<bool> AddAsync(T newObject);
        Task<bool> UpdateAsync(T updatedobject);
        Task<T> GetByNameAsync(string text);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(long id);
        Task<List<T>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(long id);
    }
}
