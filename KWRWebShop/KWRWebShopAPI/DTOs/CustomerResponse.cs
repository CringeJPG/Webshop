namespace KWRWebShopAPI.DTOs
{
    public class CustomerResponse
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public CustomerLoginResponse Login { get; set; }
    }

    public class CustomerLoginResponse
    {
        public int LoginId { get; set; }
        public string Email { get; set; } = string.Empty;
        public Role Type { get; set; } 
    }
}
