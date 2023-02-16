using KWRWebShopAPI.Database.Entities;

namespace KWRWebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            try
            {
                List<OrderResponse> orders = await _orderService.GetAllOrdersAsync();

                if (orders.Count() == 0)
                {
                    return NoContent();
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> FindOrderByIdAsync([FromRoute] int orderId)
        {
            try
            {
                LoginResponse currentUser = (LoginResponse)HttpContext.Items["User"];

                var orderResponse = await _orderService.FindOrderByIdAsync(orderId);

                if (currentUser != null && orderResponse.CustomerId != currentUser.Customer.CustomerId && currentUser.Type != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                if (orderResponse == null)
                {
                    return NotFound();
                }

                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequest newOrder)
        {
            try
            {
                LoginResponse currentUser = (LoginResponse)HttpContext.Items["User"];

                if (currentUser != null && newOrder.CustomerId != currentUser.Customer.CustomerId && currentUser.Type != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var orderResponse = await _orderService.CreateOrderAsync(newOrder);
                
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpDelete]
        [Route("{orderId}")]
        public async Task<IActionResult> DeleteOrderByIdAsync([FromRoute] int orderId)
        {
            try
            {
                LoginResponse currentUser = (LoginResponse)HttpContext.Items["User"];

                var orderResponse = await _orderService.DeleteOrderByIdAsync(orderId);

                if (currentUser != null && orderResponse.CustomerId != currentUser.Customer.CustomerId && currentUser.Type != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                if (orderResponse == null)
                {
                    return NotFound();
                }

                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpPut]
        [Route("{orderId}")]
        public async Task<IActionResult> UpdateOrderByIdAsync([FromRoute] int orderId, [FromBody] OrderRequest updateOrder)
        {
            try
            {
                LoginResponse currentUser = (LoginResponse)HttpContext.Items["User"];

                var idResponse = await _orderService.FindOrderByIdAsync(orderId);

                if (currentUser != null && idResponse.CustomerId != currentUser.Customer.CustomerId && currentUser.Type != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var orderResponse = await _orderService.UpdateOrderByIdAsync(orderId, updateOrder);

                if (orderResponse == null)
                {
                    return NotFound();
                }

                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
