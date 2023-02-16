using KWRWebShopAPI.Controllers;
using KWRWebShopAPI.DTOs;
using Microsoft.AspNetCore.Http;

namespace KWRWebShopTests.Services
{
    public class OrderServiceTests
    {
        private readonly OrderService _orderService;
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();

        public OrderServiceTests()
        {
            _orderService = new(_orderRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllOrdersAsync_ShouldReturnListOfOrderResponses_WhenOrdersExist()
        {
            // Arrange
            List<Order> orders = new()
            {
                new()
                {
                    OrderId = 1,
                    CustomerId = 1,
                    Total = 999,
                    Created = DateTime.Now
                },
                new()
                {
                    OrderId = 2,
                    CustomerId = 2,
                    Total = 9999,
                    Created = DateTime.Now
                }
            };

            _orderRepositoryMock
                .Setup(x => x.GetAllOrdersAsync())
                .ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetAllOrdersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<OrderResponse>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllOrdersAsync_ShouldReturnEmptyListOfOrderResponses_WhenNoOrdersExist()
        {
            // Arrange 
            List<Order> orders = new();

            _orderRepositoryMock
                .Setup(x => x.GetAllOrdersAsync())
                .ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetAllOrdersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<OrderResponse>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void GetAllOrdersAsync_ShouldThrowNullException_WhenRepositoryReturnsNull()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(x => x.GetAllOrdersAsync())
                .ReturnsAsync(() => throw new ArgumentNullException());

            // Act
            async Task action() => await _orderService.GetAllOrdersAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Contains("Value cannot be null", ex.Message);
        }

        [Fact]
        public async void FindOrderByIdAsync_ShouldReturnOrderResponse_WhenOrderExist()
        {
            // Arramge 
            int orderId = 1;

            Order order = new()
            {
                OrderId = orderId,
                CustomerId = 1,
                Total = 1,
                Created = DateTime.Now,
                Customer = new()
                {
                    FirstName = "test",
                    LastName = "test",
                    Address = "test",
                    Login = new()
                    {
                        Email = "test@mail.dk",
                    }
                },
                Orderline = new()
                {
                    new Orderline
                    {
                        OrderId = 1,
                        ProductId = 1,
                        OrderlineId = 1,
                        Amount = 99,
                        Price = 100
                    }
                }
            };
            
            _orderRepositoryMock
                .Setup(x => x.FindOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.FindOrderByIdAsync(orderId); 

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderResponse>(result);
            Assert.Equal(order.OrderId, result.OrderId);
            Assert.Equal(order.CustomerId, result.CustomerId);
            Assert.Equal(order.Total, result.Total);
            Assert.Equal(order.Created, result.Created);
        }

        [Fact]
        public async void FindOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(x => x.FindOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _orderService.FindOrderByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void CreateOrderAsync_ShouldReturnOrderResponse_WhenCreateIsSuccess()
        {
            // Arrange 
            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                Created = DateTime.Now

            };

            int orderId = 1;

            Order order = new()
            {
                OrderId = orderId,
                CustomerId = 1,
                Total = 999,
                Created = DateTime.Now,

                Orderline = new()
                { new Orderline
                    {
                        OrderlineId = 1,
                        OrderId = 1,
                        ProductId = 1,
                        Amount = 1,
                        Price = 999
                    }
                },
                Customer = new()
                {
                    FirstName = "test",
                    LastName = "test",
                    Address = "test",
                    Login = new()
                    {
                        Email = "test@mail.dk",
                    }
                }
            };

            _orderRepositoryMock
                .Setup(x => x.CreateOrderAsync(It.IsAny<Order>()))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.CreateOrderAsync(orderRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderResponse>(result);
            Assert.Equal(order.OrderId, result.OrderId);
            Assert.Equal(order.CustomerId, result.CustomerId);
            Assert.Equal(order.Total, result.Total);
            Assert.Equal(order.Created, result.Created);
        }

        [Fact]
        public async void CreateOrderAsync_ShouldReturnNullException_WhenRepositoryReturnsNull()
        {
            // Arrange 
            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                Created = DateTime.Now

            };

            _orderRepositoryMock
                .Setup(x => x.CreateOrderAsync(It.IsAny<Order>()))
                .ReturnsAsync(() => null);

            // Act
            async Task action() => await _orderService.CreateOrderAsync(orderRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Contains("Value cannot be null", ex.Message);
        }

        [Fact] 
        public async void DeleteOrderByIdAsync_ShouldReturnOrderResponse_WhenDeleteIsSuccess()
        {
            // Arrange
            int orderId = 1;

            Order order = new()
            {
                OrderId = orderId,
                CustomerId = 1,
                Total = 999,
                Created = DateTime.Now,
                Customer = new()
                {
                    FirstName = "test",
                    LastName = "test",
                    Address = "test",
                    Login = new()
                    {
                        Email = "test@mail.dk",
                    }
                },
                Orderline = new()
                {
                    new Orderline
                    {
                        OrderlineId = 1,
                        OrderId = 1,
                        ProductId = 1,
                        Amount = 99,
                        Price = 100
                    }
                }
            };

            _orderRepositoryMock
                .Setup(x => x.DeleteOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.DeleteOrderByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderResponse>(result);
            Assert.Equal(orderId, result.OrderId);
        }

        [Fact]
        public async void DeleteOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(x => x.DeleteOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _orderService.DeleteOrderByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateOrderByIdAsync_ShouldReturnUpdateResponse_WhenUpdateIsSuccess()
        {
            // Arrange
            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                Created = DateTime.Now
            };

            int orderId = 1;
            Order updateOrder = new()
            {
                OrderId = orderId,
                CustomerId = 1,
                Total = 999,
                Created = DateTime.Now,
                Customer = new()
                {
                    FirstName = "test",
                    LastName = "test",
                    Address = "test",
                    Login = new()
                    {
                        Email = "test@mail.dk",
                    }
                },
                Orderline = new()
                {
                    new Orderline
                    {
                        OrderlineId = 1,
                        OrderId = 1,
                        ProductId = 1,
                        Amount = 99,
                        Price = 100
                    }
                }
            };

            _orderRepositoryMock
                .Setup(x => x.UpdateOrderByIdAsync(It.IsAny<int>(), It.IsAny<Order>()))
                .ReturnsAsync(updateOrder);

            // Act
            var result = await _orderService.UpdateOrderByIdAsync(orderId, orderRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderResponse>(result);
            Assert.Equal(orderId, result.OrderId);
        }

        [Fact]
        public async void UpdateOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                Created = DateTime.Now
            };

            _orderRepositoryMock
                .Setup(x => x.UpdateOrderByIdAsync(It.IsAny<int>(), It.IsAny<Order>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _orderService.UpdateOrderByIdAsync(1, orderRequest);

            // Assert
            Assert.Null(result);
        }
    }
}
