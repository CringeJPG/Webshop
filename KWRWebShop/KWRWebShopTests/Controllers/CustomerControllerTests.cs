using KWRWebShopAPI.Controllers;
using KWRWebShopAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Moq;

namespace KWRWebShopTests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly CustomerController _customerController;
        private readonly Mock<ICustomerService> _customerServiceMock = new();

        public CustomerControllerTests()
        {
            _customerController = new(_customerServiceMock.Object);
        }

        [Fact]
        public async void GetAllCustomerAsync_ShouldReturnStatus200_WhenCustomerExists()
        {
            // Arrange
            List<CustomerResponse> customers = new();

            customers.Add(new CustomerResponse()
            {
                CustomerId = 1,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now
            });

            customers.Add(new CustomerResponse()
            {
                CustomerId = 2,
                FirstName = "Gabe",
                LastName = "Itch",
                Address = "Borgmester Christiansens Gade 22",
                Created = DateTime.Now
            });

            _customerServiceMock
                .Setup(x => x.GetAllCustomerAsync())
                .ReturnsAsync(customers);

            // Act
            var result = (IStatusCodeActionResult) await _customerController.GetAllCustomerAsync();

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void GetAllCustomerAsync_ShouldReturnStatus404_WhenNoCustomersExists()
        {
            // Arrange
            List<CustomerResponse> customers = new();

            _customerServiceMock
                .Setup(x => x.GetAllCustomerAsync())
                .ReturnsAsync(customers);

            // Act
            var result = (IStatusCodeActionResult) await (_customerController.GetAllCustomerAsync());

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void GetAllCustomerAsync_ShouldReturnStatus500_WhenExceptionIsRaised()
        {
            // Arrange
            List<CustomerResponse> customers = new();

            _customerServiceMock
                .Setup(x => x.GetAllCustomerAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)  await _customerController.GetAllCustomerAsync();

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void FindCustomerByIdAsync_ShouldReturnStatus200_WhenCustomerExists()
        {
            // Arrange
            int customerId = 1;

            CustomerResponse customer = new()
            {
                CustomerId = customerId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", customer.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _customerController.ControllerContext.HttpContext = mockHttpContext.Object;

            _customerServiceMock
                .Setup(x => x.FindCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(customer);

            // Act
            var result = (IStatusCodeActionResult) await _customerController.FindCustomerByIdAsync(customerId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void FindCustomerByIdAsync_ShouldReturnStatus404_WhenCustomerDoesNotExists()
        {
            int currentUserId = 1;

            CustomerResponse customer = new()
            {
                CustomerId = currentUserId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", customer.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _customerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Arrange
            int customerId = 1;

            _customerServiceMock
                .Setup(x => x.FindCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult) await _customerController.FindCustomerByIdAsync(customerId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void FindCustomerByIdAsync_ShouldReturnStatus500_WhenExceptionIsRaised()
        {
            // Arrange
            int customerId = 1;

            _customerServiceMock
                .Setup(x => x.FindCustomerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _customerController.FindCustomerByIdAsync(customerId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void CreateCustomerAsync_ShouldReturnStatus200_WhenCustomerSuccessfullyCreated()
        {
            // Arrange
            CustomerRequest customerRequest = new()
            {
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now,
            };

            int customerId = 1;
            CustomerResponse customerResponse = new()
            {
                CustomerId = customerId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now,
                Login = new()
                {
                    Email = "test",
                    Type = (Role)1
                }
            };

            _customerServiceMock
                .Setup(x => x.CreateCustomerAsync(It.IsAny<CustomerRequest>()))
                .ReturnsAsync(customerResponse);

            // Act
            var result = (IStatusCodeActionResult)await _customerController.CreateCustomerAsync(customerRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void CreateCustomerAsync_ShouldReturnStatus500_WhenExceptionIsRaised()
        {
            // Arrange
            CustomerRequest CustomerRequest = new()
            {
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now
            };

            _customerServiceMock
                .Setup(x => x.CreateCustomerAsync(It.IsAny<CustomerRequest>()))
                .ReturnsAsync(() => throw new Exception("This is a exception"));

            // Act
            var result = (IStatusCodeActionResult) await _customerController.CreateCustomerAsync(CustomerRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void UpdateCustomerById_ShouldReturnStatus200_WhenCustomerIsUpdated()
        {
            // Arrange
            CustomerRequest customerRequest = new()
            {
                FirstName = "Joe",
                LastName = "Mama",
                Address = "test",
                Created = DateTime.Now,
            };

            int customerId = 1;

            CustomerResponse customerResponse = new()
            {
                CustomerId = customerId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "test",
                Created = DateTime.Now,
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", customerResponse.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _customerController.ControllerContext.HttpContext = mockHttpContext.Object;

            _customerServiceMock
                .Setup(x => x.UpdateCustomerByIdAsync(It.IsAny<int>(), It.IsAny<CustomerRequest>()))
                .ReturnsAsync(customerResponse);

            // Act
            var result = (IStatusCodeActionResult)await _customerController.UpdateCustomerById(customerId, customerRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void UpdateCustomerById_ShouldReturnStatus404_WhenCustomerDoeSNotExist()
        {
            // Arrange
            int currentUserId = 1;

            CustomerResponse customer = new()
            {
                CustomerId = currentUserId,
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Langgade 1",
                Created = DateTime.Now
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", customer.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _customerController.ControllerContext.HttpContext = mockHttpContext.Object;

            CustomerRequest customerRequest = new()
            {
                FirstName = "Joe",
                LastName = "Mama",
                Address = "test",
                Created = DateTime.Now,
            };

            int customerId = 1;

            _customerServiceMock
                .Setup(x => x.UpdateCustomerByIdAsync(It.IsAny<int>(), It.IsAny<CustomerRequest>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _customerController.UpdateCustomerById(customerId, customerRequest);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void UpdateCustomerById_ShouldReturn500_WhenExceptionIsThrown()
        {
            // Arrange
            CustomerRequest customerRequest = new()
            {
                FirstName = "Joe",
                LastName = "Mama",
                Address = "Test",
                Created = DateTime.Now,
            };

            int customerId = 1;

            _customerServiceMock
                .Setup(x => x.UpdateCustomerByIdAsync(It.IsAny<int>(), It.IsAny<CustomerRequest>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _customerController.UpdateCustomerById(customerId, customerRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }
    }
}
