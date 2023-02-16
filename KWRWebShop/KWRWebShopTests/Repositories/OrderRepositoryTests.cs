using KWRWebShopAPI.Database;
using KWRWebShopAPI.Database.Entities;
using KWRWebShopAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace KWRWebShopTests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly OrderRepository _orderRepository;

        public OrderRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "OrderRepositoryTests")
                .Options;

            _context = new(_options);

            _orderRepository = new(_context);
        }

        [Fact]
        public async void GetAllOrdersAsync_ShouldReturnListOfOrders_WhenOrdersExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Order.Add(new Order
            {
                OrderId = 1,
                CustomerId = 1,
                Total = 999,
                Created = DateTime.Now,
            });
            
            _context.Order.Add(new Order
            {
                OrderId = 2,
                CustomerId = 2,
                Total = 1999,
                Created = DateTime.Now,
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _orderRepository.GetAllOrdersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Order>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllOrdersAsync_ShouldReturnEmptyList_WhenNoOrderExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _orderRepository.GetAllOrdersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Order>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void FindOrderByIdAsync_ShouldReturnOrder_WhenOrderExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;
            Customer customer = new()
            {
                CustomerId = customerId,
                FirstName = "test",
                LastName = "test",
                Address = "test",
                Created = DateTime.Now,
                Login = new Login()
                {
                    LoginId = 1,
                    Email = "test",
                    Type = Role.User
                }
            };

            int orderId = 1;
            Order order = new Order
            {
                OrderId = orderId,
                CustomerId = 1,
                Created = DateTime.Now,
                Orderline = new() { },
                Customer = customer
            };

            _context.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _orderRepository.FindOrderByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Order>(result);
            Assert.Equal(orderId, result.OrderId);
        }
        
        [Fact]
        public async void FindOrderByIdAsync_ShouldReturnNull_WhenNoOrdersExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _orderRepository.FindOrderByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void CreateOrderAsync_ShouldCreateNewOrder_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Order newOrder = new()
            { 
                CustomerId = 1,
                Total = 999,
                Created = DateTime.Now,
                Orderline =
                {
                    new Orderline
                    {
                        OrderlineId = 1,
                        OrderId = 1,
                        ProductId = 1,
                        Amount = 1,
                        Price = 999
                    }
                }
            };

            // Act
            var result = await _orderRepository.CreateOrderAsync(newOrder);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Order>(result);
            Assert.Equal(expectedId, result.OrderId);
        }

        [Fact]
        public async void CreateOrderAsync_ShouldFailToAddNewOrder_WhenOrderIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Order order = new()
            {
                CustomerId = 1,
                Total = 999,
                Created = DateTime.Now,
                Orderline =
                {
                    new Orderline
                    {
                        OrderlineId = 1,
                        OrderId = 1,
                        ProductId = 1,
                        Amount = 1,
                        Price = 999
                    }
                }
            };

            await _orderRepository.CreateOrderAsync(order);

            // Act
            async Task action() => await _orderRepository.CreateOrderAsync(order);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async void DeleteOrderByIdAsync_ShouldReturnOrder_WhenOrderIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;
            Customer customer = new()
            {
            CustomerId = customerId,
            FirstName = "test",
            LastName = "test",
            Address = "test",
            Created= DateTime.Now,
            Login = new Login()
                {
                    LoginId = 1,
                    Email = "test",
                    Type = Role.User
                }
            };

            int orderId = 1;
            Order order = new()
            {
                OrderId = orderId,
                CustomerId = 1,
                Created = DateTime.Now,
                Orderline = new() { },
                Customer = customer,
            };

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _orderRepository.DeleteOrderByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Order>(result);
            Assert.Equal(orderId, result?.OrderId);
        }

        [Fact]
        public async void DeleteOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _orderRepository.DeleteOrderByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateOrderByIdAsync_ShouldChangeValuesOnOrder_WhenOrderExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;
            Customer customer = new()
            {
                CustomerId = customerId,
                FirstName = "test",
                LastName = "test",
                Address = "test",
                Created = DateTime.Now,
                Login = new Login()
                {
                    LoginId = 1,
                    Email = "test",
                    Type = Role.User
                }
            };

            int orderId = 1;
            Order order = new()
            {
                OrderId = orderId,
                CustomerId = 1,
                Created = DateTime.Now,
                Orderline = new() { },
                Customer = customer
            };

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            Order updateOrder = new()
            {
                Orderline = new() { }
            };

            // Act
            var result = await _orderRepository.UpdateOrderByIdAsync(orderId, updateOrder);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Order>(result);
            Assert.Equal(updateOrder.Total, result.Total);
            Assert.Equal(updateOrder.Orderline, result.Orderline);
        }

        [Fact]
        public async void UpdateOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist() 
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Order updateOrder = new()
            {
                Total = 199,
                Orderline = new() { }
            };

            // Act
            var result = await _orderRepository.UpdateOrderByIdAsync(1, updateOrder);

            // Assert
            Assert.Null(result);
        }
    }
}
