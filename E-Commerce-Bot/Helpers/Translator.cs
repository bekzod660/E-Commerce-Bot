using E_Commerce_Bot.DTOs;
using E_Commerce_Bot.Entities;
using System.Reflection;

namespace E_Commerce_Bot.Helpers
{
    public static class Translator
    {
        public static List<string> Translate(string property, List<Category> translations)
        {
            List<string> result = new List<string>();

            foreach (var item in translations)
            {
                PropertyInfo prop = item.GetType().GetProperty(property);

                if (prop != null)
                {
                    object value = prop.GetValue(item);
                    if (value != null)
                    {
                        result.Add(value.ToString());
                    }
                }
                else
                {
                    throw new ArgumentException($"Property '{property}' not found in object of type '{item.GetType().Name}'.");
                }
            }
            return result;
        }
        public static List<TranslatedProduct> Translate(this List<Product> translations, string language)
        {
            List<TranslatedProduct> translatedProduct = new List<TranslatedProduct>();

            foreach (var item in translations)
            {
                string nameProperty = (item.GetType().GetProperty($"name_{language}")).GetValue(item).ToString();
                string descriptionProperty = (item.GetType().GetProperty($"description_{language}")).GetValue(item).ToString();
                if (descriptionProperty != null && nameProperty != null)
                    translatedProduct.Add(new TranslatedProduct
                    {
                        Name = nameProperty,
                        Description = descriptionProperty,
                        Price = item.Price,
                        ImagePath = item.ImagePath
                    });
                else
                {
                    throw new ArgumentException($"Property '{nameProperty}' not found in object of type '{item.GetType().Name}'.");
                }
            }
            return translatedProduct;
        }

        public static TranslatedProduct Translate(this Product product, string language)
        {
            string nameProperty = (product.GetType().GetProperty($"name_{language}")).GetValue(product).ToString();
            string descriptionProperty = (product.GetType().GetProperty($"description_{language}")).GetValue(product).ToString();
            return new TranslatedProduct
            {
                Name = nameProperty,
                Description = descriptionProperty,
                ImagePath = product.ImagePath,
                Price = product.Price,
            };
        }
    }
}
