using System.Diagnostics;

namespace KWRWebShopAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> FindOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(Order newOrder);
        Task<Order?> DeleteOrderByIdAsync(int id);
        Task<Order?> UpdateOrderByIdAsync(int id, Order updateOrder);
    }

    public class OrderRepository : IOrderRepository
    {

        private readonly DatabaseContext _context;

        public OrderRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Order
                .Include(o => o.Orderline)
                .ToListAsync();
        }

        public async Task<Order?> FindOrderByIdAsync(int id)
        {
            return await _context.Order
                .Include(o => o.Orderline)
                .Include(o => o.Customer)
                .Include(o => o.Customer.Login)
                .FirstOrDefaultAsync(x => x.OrderId == id);
        }

        public async Task<Order> CreateOrderAsync(Order newOrder)
        {
            _context.Order.Add(newOrder);
            await _context.SaveChangesAsync();
            return newOrder;
        }

        public async Task<Order?> DeleteOrderByIdAsync(int id)
        {
            var order = await FindOrderByIdAsync(id);
            
            if (order != null)
            {
                _context.Remove(order);
                await _context.SaveChangesAsync();
            }

            return order;
        }

        public async Task<Order?> UpdateOrderByIdAsync(int id, Order updateOrder)
        {
            var order = await FindOrderByIdAsync(id);

            if (order != null)
            {
                order.Total = updateOrder.Total;
                order.Orderline = updateOrder.Orderline;

                await _context.SaveChangesAsync();
                order = await FindOrderByIdAsync(order.OrderId);
            }

            return order;
        }
    }
}
