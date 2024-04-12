using E_Commerce_Bot.Entities;

namespace E_Commerce_Bot.Persistence.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductByLanguageAsync(string language);
        Task<List<Product>> GetProductsByLanguageAsync(string language);
    }
}
