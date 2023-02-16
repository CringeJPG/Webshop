namespace KWRWebShopAPI.Database.Entities
{
    public class Orderline
    {
        [Key]
        public int OrderlineId { get; set; }

        [ForeignKey("Order.OrderId")]
        public int OrderId { get; set; }

        [ForeignKey("Product.ProductId")]
        public int ProductId { get; set; }

        [Column(TypeName = "int")]
        public int Amount { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
    }
}
