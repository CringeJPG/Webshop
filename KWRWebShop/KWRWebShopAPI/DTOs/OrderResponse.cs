namespace KWRWebShopAPI.DTOs
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
        public DateTime Created { get; set; }

        public OrderCustomerResponse? Customer { get; set; } = new();
        public List<OrderOrderlineResponse> Orderline { get; set; } = new();
    }

    public class OrderOrderlineResponse
    {
        public int OrderlineId { get; set; }
        public int OrderId { get; set; } = 0;
        public int ProductId { get; set; } = 0;
        public int Amount { get; set; } = 0;
        public decimal Price { get; set; } = 0;
    }

    public class OrderCustomerResponse
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public OrderCustomerLoginResponse? Login { get; set; }
    }

    public class OrderCustomerLoginResponse
    {
        public string? Email { get; set; }
    }
}
