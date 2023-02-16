namespace KWRWebShopAPI.DTOs
{
    public class ProductRequest
    {
        [Required]
        [StringLength(32, ErrorMessage ="Product name cannot be longer than 32 chars")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(32, ErrorMessage = "Brand cannot be longer than 32 chars")]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(32, ErrorMessage = "Description cannot be longer than 32 chars")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, 10000, ErrorMessage = "Price has to be between 0 and 10000")]
        public short Price { get; set; }

        [Required(ErrorMessage="Category is required")]
        [Range(1, int.MaxValue, ErrorMessage ="Team must not be 0")]
        public int CategoryId { get; set; }


    }
}
