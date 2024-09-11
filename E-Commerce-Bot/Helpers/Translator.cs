using E_Commerce_Bot.DTOs;
using E_Commerce_Bot.Entities;
using System.Reflection;

namespace E_Commerce_Bot.Helpers
{
    public static class Translator
    {
        public static List<string> Translate(string language, List<Category> translations)
        {
            List<string> result = new List<string>();
            string property = GetPropertyName(language);

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
        public static List<string> Translate(string language, List<Product> translations)
        {
            List<string> result = new List<string>();
            string property = GetPropertyName(language);
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

        public static ProductDto Translate(this Product product, string language)
        {
            string property = GetPropertyName(language);
            string propertyDes = GetPropertyDescription(language);
            string nameProperty = (product.GetType().GetProperty(property)).GetValue(product).ToString();
            string descriptionProperty = (product.GetType().GetProperty(propertyDes)).GetValue(product).ToString();
            return new ProductDto
            {
                Name = nameProperty,
                Description = descriptionProperty,
                ImagePath = product.ImagePath,
                Price = product.Price,
            };
        }
        private static string GetPropertyName(string language)
        {
            return language switch
            {
                "uz" => "Name_Uz",
                "eu" => "Name_Ru",
                "en" => "Name_En",
            };
        }
        private static string GetPropertyDescription(string language)
        {
            return language switch
            {
                "uz" => "Description_Uz",
                "eu" => "Description_Ru",
                "en" => "Description_En",
            };
        }
    }
}
