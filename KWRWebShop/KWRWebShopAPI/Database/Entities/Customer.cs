namespace KWRWebShopAPI.Database.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [ForeignKey("Login.LoginId")]
        public int LoginId { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string FirstName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(32)")]
        public string LastName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(64)")]
        public string Address { get; set; } = string.Empty;

        [Column(TypeName = "datetime")]
        public DateTime Created { get; set; } = DateTime.Now;

        public Login Login { get; set; }
    }
}
