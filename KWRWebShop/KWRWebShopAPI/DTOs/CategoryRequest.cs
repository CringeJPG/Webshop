namespace KWRWebShopAPI.DTOs
{
    public class CategoryRequest
    {
        [Required]
        [StringLength(32, ErrorMessage = "CategoryName cannot be longer than 32 chars")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
