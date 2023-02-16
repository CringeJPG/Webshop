using KWRWebShopAPI.Controllers;
using KWRWebShopAPI.Database.Entities;
using KWRWebShopAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace KWRWebShopTests.Controller
{
    public class OrderControllerTests
    {
        private readonly OrderController _orderController;
        private readonly Mock<IOrderService> _orderServiceMock = new();

        public OrderControllerTests()
        {
            _orderController = new(_orderServiceMock.Object);
        }

        [Fact]
        public async void GetAllOrdersAsync_ShouldReturnStatusCode200_WhenOrdersExists()
        {
            // Arrange
            List<OrderResponse> orders = new()
            {
                new()
                {
                    OrderId = 1,
                    CustomerId = 1,
                    Total = 999,
                    Created = DateTime.Now,
                },
                new()
                {
                    OrderId = 2,
                    CustomerId = 2,
                    Total = 1999,
                    Created = DateTime.Now,
                }
            };

            _orderServiceMock
                .Setup(x => x.GetAllOrdersAsync())
                .ReturnsAsync(orders);

            // Act
            var result = (IStatusCodeActionResult)await _orderController.GetAllOrdersAsync();

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void GetAllOrdersAsync_ShouldReturnStatusCode204_WhenNoOrdersExists()
        {
            // Arrange
            List<OrderResponse> orders = new();

            _orderServiceMock
                .Setup(x => x.GetAllOrdersAsync())
                .ReturnsAsync(orders);

            // Act
            var result = (IStatusCodeActionResult)await _orderController.GetAllOrdersAsync();

            // Assert
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async void GetAllOrdersAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            List<OrderResponse> orders = new();

            _orderServiceMock
                .Setup(x => x.GetAllOrdersAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _orderController.GetAllOrdersAsync();

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void FindOrderByIdAsync_ShouldReturnStatusCode200_WhenOrderExists()
        {
            // Arrange
            int orderId = 1;

            OrderResponse orderResponse = new()
            {
                OrderId = orderId,
                CustomerId = 1,
                Total = 999,
                Created = DateTime.Now
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", orderResponse.Customer.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            _orderServiceMock
                .Setup(x => x.FindOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(orderResponse);

            // Act
            var result = (IStatusCodeActionResult)await _orderController.FindOrderByIdAsync(orderId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void FindOrderByIdAsync_ShouldReturnStatusCode404_WhenOrderDoesNotExist()
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
            _orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            int orderId = 1;

            _orderServiceMock
                .Setup(x => x.FindOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _orderController.FindOrderByIdAsync(orderId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void FindOrderByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int orderId = 1;

            _orderServiceMock
                .Setup(x => x.FindOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _orderController.FindOrderByIdAsync(orderId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void CreateOrderAsync_ShouldReturnStatusCode200_WhenOrderIsSuccessfullyCreated()
        {
            // Arrange
            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                Created = DateTime.Now,
            };

            int orderId = 1;

            OrderResponse orderResponse = new()
            {
                OrderId = orderId,
                CustomerId = 1,
                Created = DateTime.Now,
                Orderline =
                {

                }
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", orderResponse.Customer.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            _orderServiceMock
                .Setup(x => x.CreateOrderAsync(It.IsAny<OrderRequest>()))
                .ReturnsAsync(orderResponse);

            // Act
            var result = (IStatusCodeActionResult)await _orderController.CreateOrderAsync(orderRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void CreateOrderAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange 
            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                Created = DateTime.Now
            };

            _orderServiceMock
                .Setup(x => x.CreateOrderAsync(It.IsAny<OrderRequest>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _orderController.CreateOrderAsync(orderRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void DeleteOrderByIdAsync_ShouldReturnStatusCode200_WhenOrderExists()
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
            _orderController.ControllerContext.HttpContext = mockHttpContext.Object;


            int orderId = 1;

            OrderResponse orderResponse = new()
            {
                OrderId = orderId,
                CustomerId = 1,
                Total = 999,
                Created = DateTime.Now,
                Orderline = new() { }
            };

            _orderServiceMock
                .Setup(x => x.DeleteOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(orderResponse);

            // Act
            var result = (IStatusCodeActionResult)await _orderController.DeleteOrderByIdAsync(orderId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void DeleteOrderByIdAsync_ShouldReturnStatusCode404_WhenOrderDoesNotExist()
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
            _orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            _orderServiceMock
                .Setup(x => x.DeleteOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _orderController.DeleteOrderByIdAsync(1);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void DeleteOrderByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _orderServiceMock
                .Setup(x => x.DeleteOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("There is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _orderController.DeleteOrderByIdAsync(1);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void UpdateOrderByIdAsync_ShouldReturnStatusCode200_WhenOrderIsUpdated()
        {
            // Arrange
            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                Created = DateTime.Now
            };

            int orderId = 1;

            OrderResponse orderResponse = new()
            {
                OrderId = orderId,
                CustomerId = 1,
                Total = 899,
                Created = DateTime.Now,
                Orderline = new() { }
            };

            // Create a mock HTTP context and set the "User" item in the Items collection
            var mockHttpContext = new Mock<HttpContext>();
            _ = mockHttpContext.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>()
                {
                    { "User", orderResponse.Customer.Login }
                });

            // Set the mock HTTP context on the LoginController instance
            _orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            _orderServiceMock
                .Setup(x => x.UpdateOrderByIdAsync(It.IsAny<int>(), It.IsAny<OrderRequest>()))
                .ReturnsAsync(orderResponse);

            // Act
            var result = (IStatusCodeActionResult)await _orderController.UpdateOrderByIdAsync(orderId, orderRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void UpdateOrderByIdAsync_ShouldReturnStatusCode404_WhenOrderDoesNotExist()
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
            _orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                Created = DateTime.Now
            };

            _orderServiceMock
                .Setup(x => x.UpdateOrderByIdAsync(It.IsAny<int>(), It.IsAny<OrderRequest>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _orderController.UpdateOrderByIdAsync(1, orderRequest);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void UpdateOrderByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                Created = DateTime.Now
            };

            _orderServiceMock
                .Setup(x => x.UpdateOrderByIdAsync(It.IsAny<int>(), It.IsAny<OrderRequest>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _orderController.UpdateOrderByIdAsync(1, orderRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }
    }
}
