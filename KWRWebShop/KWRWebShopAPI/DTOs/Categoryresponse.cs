namespace KWRWebShopAPI.DTOs
{

        public class CategoryResponse
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public List<CategoryProductResponse> Products { get; set; } = new();
        }

        public class CategoryProductResponse
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string Brand { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
        }
    
}
