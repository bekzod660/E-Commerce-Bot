using E_Commerce_Bot.Entities;

namespace E_Commerce_Bot.Persistence.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByLanguageAsync(string languageCode);
        Task<List<Category>> GetCategoriesByLanguageAsync(string languageCode);
    }
}
