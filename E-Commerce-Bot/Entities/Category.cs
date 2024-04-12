using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Bot.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Column("name_uz")]
        public string Name_Uz { get; set; }

        [Column("name_ru")]
        public string Name_Ru { get; set; }

        [Column("name_en")]
        public string Name_En { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
