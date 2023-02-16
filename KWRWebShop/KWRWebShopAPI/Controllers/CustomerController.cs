using KWRWebShopAPI.Database.Entities;

namespace KWRWebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomerAsync()
        {
            try
            {
                List<CustomerResponse> customers = await _customerService.GetAllCustomerAsync();

                if (customers.Count == 0) 
                {
                
                    return NotFound();
                }

                return Ok(customers);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpGet]
        [Route("{customerId}")]
        public async Task<IActionResult> FindCustomerByIdAsync([FromRoute] int customerId)
        {
            try
            {
                LoginResponse currentUser = (LoginResponse)HttpContext.Items["User"];

                if (currentUser != null && customerId != currentUser.Customer.CustomerId && currentUser.Type != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var customerResponse = await _customerService.FindCustomerByIdAsync(customerId);

                if (customerResponse == null)
                {
                    return NotFound();
                }

                return Ok(customerResponse);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerRequest newCustomer)
        {
            try
            {
                CustomerResponse customerResponse = await _customerService.CreateCustomerAsync(newCustomer);

                return Ok(customerResponse);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpPut]
        [Route("{customerId}")]
        public async Task<IActionResult> UpdateCustomerById([FromRoute] int customerId, [FromBody] CustomerRequest updatedCustomer)
        {
            try
            {
                LoginResponse currentUser = (LoginResponse)HttpContext.Items["User"];

                if (currentUser != null && customerId != currentUser.Customer.CustomerId && currentUser.Type != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var customerResponse = await _customerService.UpdateCustomerByIdAsync(customerId, updatedCustomer);

                if (customerResponse == null)
                {
                    return NotFound();
                }

                return Ok(customerResponse);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpDelete]
        [Route("{customerId}")]
        public async Task<IActionResult> DeleteCustomerById([FromRoute] int customerId)
        {
            try
            {
                LoginResponse currentUser = (LoginResponse)HttpContext.Items["User"];

                if (currentUser != null && customerId != currentUser.Customer.CustomerId && currentUser.Type != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var customer = await _customerService.DeleteCustomerByIdAsync(customerId);

                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
                throw;
            }
        }
    }
}
