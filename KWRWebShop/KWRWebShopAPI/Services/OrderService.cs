namespace KWRWebShopAPI.Services
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetAllOrdersAsync();
        Task<OrderResponse?> FindOrderByIdAsync(int id);
        Task<OrderResponse?> CreateOrderAsync(OrderRequest newOrder);
        Task<OrderResponse?> DeleteOrderByIdAsync(int id);
        Task<OrderResponse?> UpdateOrderByIdAsync(int id, OrderRequest updateOrder);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderResponse>> GetAllOrdersAsync()
        {
            List<Order> orders = await _orderRepository.GetAllOrdersAsync();

            if(orders == null)
            {
                throw new ArgumentNullException();
            }

            return orders.Select(order => MapOrderToOrderResponseLimited(order)).ToList();
        }

        public async Task<OrderResponse?> FindOrderByIdAsync(int id)
        {
            var order = await _orderRepository.FindOrderByIdAsync(id);

            if (order == null)
            {
                return null;
            }

            return MapOrderToOrderResponse(order);
        }

        private OrderResponse MapOrderToOrderResponse(Order order)
        {
            return new OrderResponse
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                Total = order.Total,
                Created = order.Created,
                Customer = new OrderCustomerResponse
                {
                    FirstName = order.Customer.FirstName,
                    LastName = order.Customer.LastName,
                    Address = order.Customer.Address,
                    Login = new OrderCustomerLoginResponse
                    {
                        Email = order.Customer.Login.Email
                    }
                },
                Orderline = order.Orderline.Select(x => new OrderOrderlineResponse
                {
                    OrderlineId = x.OrderlineId,
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    Amount = x.Amount,
                    Price = x.Price,
                }).ToList()
            };
        }

        private OrderResponse MapOrderToOrderResponseLimited(Order order)
        {
            return new OrderResponse
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                Total = order.Total,
                Created = order.Created,
                Orderline = order.Orderline.Select(x => new OrderOrderlineResponse
                {
                    OrderlineId = x.OrderlineId,
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    Amount = x.Amount,
                    Price = x.Price,
                }).ToList()
            };
        }

        private Order MapOrderRequestToOrder(OrderRequest orderRequest)
        {
            return new Order
            {
                CustomerId = orderRequest.CustomerId,
                Created = orderRequest.Created,
                Orderline = orderRequest.Orderline
            };
        }

        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest newOrder)
        {
            var order = await _orderRepository.CreateOrderAsync(MapOrderRequestToOrder(newOrder));

            if (order == null) 
            {
                throw new ArgumentNullException();
            }

            return MapOrderToOrderResponse(order);
        }

        public async Task<OrderResponse?> DeleteOrderByIdAsync(int id)
        {
            var order = await _orderRepository.DeleteOrderByIdAsync(id);

            if (order == null)
            {
                return null;
            }

            return MapOrderToOrderResponse(order);
        }

        public async Task<OrderResponse?> UpdateOrderByIdAsync(int id, OrderRequest updateOrder)
        {
            var order = await _orderRepository.UpdateOrderByIdAsync(id, MapOrderRequestToOrder(updateOrder));

            if (order == null)
            {
                return null;
            }

            return MapOrderToOrderResponse(order);
        }
    }
}
