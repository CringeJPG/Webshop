namespace KWRWebShopAPI.Database.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [ForeignKey("Category.CategoryId")]
        public int CategoryId { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(32)")]
        public string Brand { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(32)")]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public int Price { get; set; } = 0;

        public Category Category { get; set; }
    }
}
