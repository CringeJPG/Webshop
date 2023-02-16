using System.ComponentModel.DataAnnotations.Schema;

namespace KWRWebShopAPI.Database.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string CategoryName { get; set; } = string.Empty;

        public List<Product> Products { get; set; } = new();
    }
}
