namespace KWRWebShopTests.Repositories
{
    public class CustomerRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly CustomerRepository _customerRepository;

        public CustomerRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "CustomerRepositoryTests")
                .Options;

            _context = new(_options);

            _customerRepository = new(_context);
        }

        [Fact]
        public async void GetAllCustomerAsync_ShouldReturnListOfCustomers_WhenCustomersExists()
        {
            // Arrange

            await _context.Database.EnsureDeletedAsync();

            _context.Customer.Add(new Customer
            {
                CustomerId = 1,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now,
                Login = new Login()
                {
                    LoginId = 1,
                    Email = "TEST_1",
                    Type = 0,
                }
            });

            _context.Customer.Add(new Customer
            {
                CustomerId = 2,
                FirstName = "Gabe",
                LastName = "Itch",
                Address = "Borgmester Christiansens Gade 22",
                Created = DateTime.Now,
                Login = new Login()
                {
                    LoginId = 2,
                    Email = "TEST_2",
                    Type = Role.User,
                }
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _customerRepository.GetAllCustomerAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Customer>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllCustomerAsync_ShouldReturnEmptyListOfCustomers_WhenNoCustomersExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _customerRepository.GetAllCustomerAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Customer>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void FindCustomerByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;

            _context.Customer.Add(new Customer
            {
                CustomerId = 1,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now,
                Login = new Login()
                {
                    LoginId = 1,
                    Email = "TEST_1",
                    Type = 0,
                }
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _customerRepository.FindCustomerByIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
            Assert.Equal(customerId, result.CustomerId);
        }

        [Fact]
        public async void FindCustomerByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _customerRepository.FindCustomerByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void CreateCustomerAsync_ShouldAddNewCustomer_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Customer customer = new()
            {
                CustomerId = 1,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now,
                Login = new Login()
                {
                    LoginId = 1,
                    Email = "TEST_1",
                    Type = 0,
                }
            };

            // Act
            var result = await _customerRepository.CreateCustomerAsync(customer);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
            Assert.Equal(expectedId, result.CustomerId);
        }

        [Fact]
        public async void CreateCustomerAsync_ShouldFailToAddNewCustomer_WhenCustomerIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var email = "test@mail.dk";

            Customer customer = new()
            {
                CustomerId = 1,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now,
                Login = new()
                {
                    LoginId = 1,
                    Email = email
                }
            };

            await _customerRepository.CreateCustomerAsync(customer);

            // Act
            async Task action() => await _customerRepository.CreateCustomerAsync(customer);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(action);
            Assert.Contains("The email " + email + " is not available", ex.Message);

        }

        [Fact]
        public async void UpdateCustomerByIdAsync_ShouldChangeValuesForCustomer_WhenCustomerExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;

            Customer newCustomer = new()
            {
                CustomerId = customerId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Testallé 5",
                Login = new()
                {
                    Email = "test",
                }
            };
            _context.Customer.Add(newCustomer);
            await _context.SaveChangesAsync();

            Customer updatedCustomer = new()
            {
                CustomerId = customerId,
                FirstName = "new Joe",
                LastName = "new Mama",
                Address = "new Testallé 5",
                Login = new()
                {
                    Email = "test",
                }
            };

            // Act
            var result = await _customerRepository.UpdateCustomerByIdAsync(customerId, updatedCustomer);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
            Assert.Equal(customerId, result?.LoginId);
            Assert.Equal(updatedCustomer.FirstName, result?.FirstName);
            Assert.Equal(updatedCustomer.LastName, result?.LastName);
            Assert.Equal(updatedCustomer.Address, result?.Address);
            Assert.Equal(updatedCustomer.Login.Type, result?.Login.Type);
        }

        [Fact]
        public async void UpdateCustomerByIdAsync_ShouldReturnNull_IfCustomerDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;
            Customer updatedCustomer = new()
            {
                CustomerId = customerId,
                FirstName = "Joe",
                LastName = "Mama",
                Address =  "Test"
            };

            // Act
            var result = await _customerRepository.UpdateCustomerByIdAsync(customerId, updatedCustomer);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteCustomerByIdAsync_ShouldReturnDeletedCustomer_WhenCustomerIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;
            Customer customer = new()
            {
                CustomerId = 1,
                FirstName = "Joe",
                LastName = "Mama",
                Created = DateTime.Now,
                Login = new()
                {
                    Email = "Test",
                    Type = 0
                }
            };

            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerRepository.DeleteCustomerByIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
            Assert.Equal(customerId, result?.CustomerId);
        }

        [Fact]
        public async void DeleteCustomerByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _customerRepository.DeleteCustomerByIdAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}
