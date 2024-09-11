using E_Commerce_Bot.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Bot.Persistence.Repositories
{
    public class ProductRepository : IBaseRepository<Product>
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddAsync(Product obj)
        {
            _db.Products.Add(obj);
            return await _db.SaveChangesAsync() > 0;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByNameAsync(string text)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByNameAsync(string text, string language)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetProductByLanguageAsync(int id, string language)
        {
            string nameField, descriptionField;
            switch (language.ToLower())
            {
                case "ru":
                    nameField = nameof(Product.Name_Ru);
                    descriptionField = nameof(Product.Description_Ru);
                    break;
                case "uz":
                    nameField = nameof(Product.Name_Uz);
                    descriptionField = nameof(Product.Description_Uz);
                    break;
                case "en":
                default:
                    nameField = nameof(Product.Name_En);
                    descriptionField = nameof(Product.Description_En);
                    break;
            }

            var product = await _db.Products
                .Where(p => p.Id == id)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name_Uz = language.ToLower() == "uz" ? p.Name_Uz : null,
                    Name_Ru = language.ToLower() == "ru" ? p.Name_Ru : null,
                    Name_En = language.ToLower() == "en" ? p.Name_En : null,
                    Description_Uz = language.ToLower() == "uz" ? p.Description_Uz : null,
                    Description_Ru = language.ToLower() == "ru" ? p.Description_Ru : null,
                    Description_En = language.ToLower() == "en" ? p.Description_En : null
                })
                .FirstOrDefaultAsync();

            return product;
        }

        public Task<List<Product>> GetProductsByLanguageAsync(string language)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Product obj)
        {
            throw new NotImplementedException();
        }
    }
}
