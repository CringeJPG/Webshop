namespace KWRWebShopAPI.Database.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("Customer.CustomerId")]
        public int CustomerId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Created { get; set; }

        public Customer Customer { get; set; }

        public List<Orderline> Orderline { get; set; } = new();

    }
}
