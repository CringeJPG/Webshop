using AutoFixture;
using KWRWebShopAPI.Database.Entities;

namespace KWRWebShopTests.Services
{
    public class CustomerServiceTests
    {
        private readonly CustomerService _customerService;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();

        public CustomerServiceTests()
        {
            _customerService = new(_customerRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllCustomerAsync_ShoudReturnListOfCustomerResponses_WhenCustomersExists()
        {

            // Arrange
            List<Customer> customers = new()
            {
                new()
                {
                    CustomerId = 1,
                    FirstName = "Joe",
                    LastName = "Mama",
                    Address = "Langgade 1",
                    Created = DateTime.Now,
                    Login = new Login()
                    {
                        LoginId = 1,
                        Email = "TEST_2",
                        Type = 0,
                    }
                },
                new()
                {   
                    CustomerId = 2,
                    FirstName = "Gabe",
                    LastName = "Itch",
                    Address = "Borgmester Christiansens Gade 22",
                    Created = DateTime.Now,
                    Login = new Login()
                    {
                        LoginId = 1,
                        Email = "TEST_1",
                        Type = Role.User,
                    }
                }
            };

            _customerRepositoryMock
                .Setup(x => x.GetAllCustomerAsync())
                .ReturnsAsync(customers);

            // Act
            var result = await _customerService.GetAllCustomerAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CustomerResponse>>(result);
            Assert.Equal(2, customers.Count);
        }

        [Fact]
        public async void GetAllCustomerAsync_ShouldReturnEmptyListOfCustomerResponses_WhenNoCustomersExist()
        {
            // Arrange
            List<Customer> customers = new();

            _customerRepositoryMock
                .Setup(x => x.GetAllCustomerAsync())
                .ReturnsAsync(customers);

            // Act
            var result = await _customerService.GetAllCustomerAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CustomerResponse>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void GetAllCustomerAsync_ShouldThrowNullException_WhenRepositoryReturnsNull()
        {
            // Arrange
            List<Customer> customers= new();

            _customerRepositoryMock
                .Setup(x => x.GetAllCustomerAsync())
                .ReturnsAsync(() => throw new ArgumentNullException());

            // Act
            async Task action() => await _customerService.GetAllCustomerAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Contains("Value cannot be null", ex.Message);
        }

        [Fact]
        public async void FindCustomerByIdAsync_ShouldReturnCustomerResponse_WhenCustomerExists()
        {
            // Arrange
            int customerId = 1;
            Customer customer = new()
            {
                CustomerId = customerId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now,
                Login = new Login()
                {
                    LoginId = 1,
                    Email = "TEST1",
                    Type = Role.User,
                }
            };

            _customerRepositoryMock
                .Setup(x => x.FindCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _customerService.FindCustomerByIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CustomerResponse>(result);
            Assert.Equal(customerId, result?.CustomerId);
            Assert.Equal(customer.FirstName, result?.FirstName);
            Assert.Equal(customer.LastName, result?.LastName);
            Assert.Equal(customer.Address, result?.Address);
            Assert.Equal(customer.Created, result?.Created);
        }

        [Fact]
        public async void FindCustomerByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            int customerId = 1;

            _customerRepositoryMock
                .Setup(x => x.FindCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _customerService.FindCustomerByIdAsync(customerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void CreateCustomerAsync_ShouldReturnCustomerResponse_WhenCreateIsSuccess()
        {
            // Arrange
            CustomerRequest customerRequest = new()
            {
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now,
                Login = new()
                {
                    Email = "test@test.dk",
                    Type = 0,
                    Password = "123"
                }
            };

            int customerId = 1;
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
                    Email = "TEST_2",
                    Type = 0,
                    Password = "123"
                }
            };

            _customerRepositoryMock
                .Setup(x => x.CreateCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _customerService.CreateCustomerAsync(customerRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CustomerResponse>(result);
            Assert.Equal(customerId, result?.CustomerId);
            Assert.Equal(customer.FirstName, result?.FirstName);
            Assert.Equal(customer.LastName, result?.LastName);
            Assert.Equal(customer.Address, result?.Address);
            Assert.Equal(customer.Created, result?.Created);
        }

        [Fact]
        public async void CreateCustomerAsync_ShouldThrowNullException_WhenRepositroyReturnsNull()
        {
            // Arrange
            CustomerRequest customerRequest = new()
            {
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now,
                Login = new()
                {
                    Type = 0,
                    Password = "123",
                    Email = "test@test.dk"
                }
            };

            _customerRepositoryMock
                .Setup(x => x.CreateCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync(() => null);

            // Act
            async Task action() => await _customerService.CreateCustomerAsync(customerRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Contains("Value cannot be null", ex.Message);
        }

        [Fact]
        public async void UpdateCustomerByIdAsync_ShouldReturnCustomerResponse_WhenUpdateIsSuccessfull()
        {
            // Arrange
            CustomerRequest customerRequest = new()
            {
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Test",
                Created = DateTime.Now,
                Login = new()
                {
                    Type = Role.Admin,
                    Password = "123",
                    Email = "test@test.dk"
                }
            };

            int customerId = 1;
            Customer customer = new() 
            { 
                CustomerId = customerId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Test",
                Created = DateTime.Now,
                Login = new()
                {
                    Email = "Test",
                    Type = Role.User
                }
            };

            _customerRepositoryMock
                .Setup(x => x.UpdateCustomerByIdAsync(It.IsAny<int>(), It.IsAny<Customer>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _customerService.UpdateCustomerByIdAsync(customerId, customerRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CustomerResponse>(result);
            Assert.Equal(customerRequest.FirstName, result.FirstName);
            Assert.Equal(customerRequest.LastName, result.LastName);
            Assert.Equal(customerRequest.Address, result.Address);
        }

        [Fact]
        public async void UpdateCustomerByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            CustomerRequest customerRequest = new()
            {
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Test",
                Created = DateTime.Now,
                Login = new()
                {
                    Type = Role.Admin,
                    Password = "123",
                    Email = "test@test.dk"
                }
            };

            int customerId = 1;

            _customerRepositoryMock
                .Setup(x => x.UpdateCustomerByIdAsync(It.IsAny<int>(), It.IsAny<Customer>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _customerService.UpdateCustomerByIdAsync(customerId, customerRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteCustomerById_ShouldReturnCustomerResponse_WhenDeleteIsSuccessfull()
        {
            // Arrange
            int customerId = 1;

            Customer customer = new()
            {
                CustomerId = customerId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Test",
                Created = DateTime.Now,
                Login = new()
                {
                    Email = "test",
                    Type = Role.User
                }
            };

            _customerRepositoryMock
                .Setup(x => x.DeleteCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _customerService.DeleteCustomerByIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CustomerResponse>(result);
            Assert.Equal(customer.CustomerId, result?.CustomerId);
        }

        [Fact]
        public async void DeleteCustomerById_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            int customerId = 1;

            _customerRepositoryMock
                .Setup(x => x.DeleteCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _customerService.DeleteCustomerByIdAsync(customerId);

            // Assert
            Assert.Null(result);
        }
    }
}
