namespace KWRWebShopAPI.DTOs
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public ProductCategoryResponse Category { get; set; } = new(); 
        
    }

    public class ProductCategoryResponse
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
