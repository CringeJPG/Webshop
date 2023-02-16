namespace KWRWebShopAPI.DTOs
{
    public class OrderRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must not be 0")] 
        public int CustomerId { get; set;}

        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public List<Orderline> Orderline { get; set; } = new();
    }
}