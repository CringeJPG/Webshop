namespace KWRWebShopAPI.Database.Entities
{
    public class Login
    {
        [Key]
        public int LoginId { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string Email { get; set; } = string.Empty;

        [Column(TypeName = "int")]
        public Role Type { get; set; } = 0;

        [Column(TypeName = "nvarchar(32)")]
        public string Password { get; set; } = string.Empty;

        public Customer? Customer { get; set; }
    }
}
