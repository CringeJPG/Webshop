namespace KWRWebShopAPI.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Orderline> OrderLine { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Våben"
                },
                
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Våbentilbehør"
                });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    CategoryId = 1,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                },
                new Product
                {
                    ProductId = 2,
                    CategoryId = 2,
                    Name = "AK47",
                    Brand = "CYMA",
                    Description = "Fuld automatisk assault rifle.",
                    Price = 1599
                });

            modelBuilder.Entity<Login>().HasData(
                new Login
                {
                    LoginId = 1,
                    Email = "mail_1@test.dk",
                    Type = 0,
                    Password = "123456789"
                },
                new Login
                {
                    LoginId = 2,
                    Email = "mail_2@test.dk",
                    Type = (Role)1,
                    Password = "123456789"
                });

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    LoginId = 1,
                    FirstName = "Joe",
                    LastName = "Mama",
                    Address = "Langgade 1",
                    Created = DateTime.Now
                },
                new Customer
                {
                    CustomerId = 2,
                    LoginId = 2,
                    FirstName = "Gabe",
                    LastName = "Itch",
                    Address = "Borgmester Christiansens Gade 22",
                    Created = DateTime.Now
                });

            modelBuilder.Entity<Orderline>().HasData(
                new Orderline
                {
                    OrderlineId = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Amount = 1,
                    Price = 899
                },
                new Orderline
                {
                    OrderlineId = 2,
                    OrderId = 1,
                    ProductId = 1,
                    Amount = 1,
                    Price = 999
                },
                new Orderline
                {
                    OrderlineId = 3,
                    OrderId = 2,
                    ProductId = 2,
                    Amount = 1,
                    Price = 1599
                });

            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    OrderId = 1,
                    CustomerId = 1,
                    Total = 1898,
                    Created = DateTime.Now
                },
                new Order
                {
                    OrderId = 2,
                    CustomerId = 2,
                    Total = 1599,
                    Created = DateTime.Now
                });
        }
    }
}
