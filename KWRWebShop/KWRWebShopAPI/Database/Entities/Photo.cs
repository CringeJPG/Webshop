namespace KWRWebShopAPI.Database.Entities
{
    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(32)")]
        public string Path { get; set; } = string.Empty;

        [ForeignKey("Product.ProductId")]
        public int ProductId { get; set; }
    }
}
