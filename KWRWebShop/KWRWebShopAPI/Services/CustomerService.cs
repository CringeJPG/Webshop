namespace KWRWebShopAPI.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerResponse>> GetAllCustomerAsync();
        Task<CustomerResponse?> FindCustomerByIdAsync(int customerId);
        Task<CustomerResponse> CreateCustomerAsync(CustomerRequest newCustomer);
        Task<CustomerResponse?> UpdateCustomerByIdAsync(int customerId, CustomerRequest updatedCustomer);
        Task<CustomerResponse?> DeleteCustomerByIdAsync(int customerId);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        private static CustomerResponse MapCustomerToCustomerResponse(Customer customer)
        {
            return new CustomerResponse
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Created = customer.Created,
                Login = new CustomerLoginResponse
                {
                    LoginId = customer.Login.LoginId,
                    Email = customer.Login.Email,
                    Type = customer.Login.Type
                }
            };
        }

        private static Customer MapCustomerRequestToCustomer(CustomerRequest customerRequest)
        {
            return new Customer
            {
                FirstName = customerRequest.FirstName,
                LastName = customerRequest.LastName,
                Address = customerRequest.Address,
                Created = customerRequest.Created,
                Login = new()
                {
                    Email = customerRequest.Login.Email,
                    Password = customerRequest.Login.Password,
                    Type = customerRequest.Login.Type
                }
            };
        }

        public async Task<List<CustomerResponse>> GetAllCustomerAsync()
        {
            List<Customer> customers = await _customerRepository.GetAllCustomerAsync();

            if (customers == null)
            {
                throw new ArgumentNullException();
            }

            return customers.Select(customer => MapCustomerToCustomerResponse(customer)).ToList();
        }

        public async Task<CustomerResponse?> FindCustomerByIdAsync(int customerId)
        {
            var customer = await _customerRepository.FindCustomerByIdAsync(customerId);

            if (customer != null)
            {
                return MapCustomerToCustomerResponse(customer);
            }

            return null;
        }

        public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest newCustomer)
        {
            var customer = await _customerRepository.CreateCustomerAsync(MapCustomerRequestToCustomer(newCustomer));

            if (customer == null)
            {
                throw new ArgumentNullException();
            }

            return MapCustomerToCustomerResponse(customer);
        }

        public async Task<CustomerResponse?> UpdateCustomerByIdAsync(int customerId, CustomerRequest updatedCustomer)
        {
            var customer = await _customerRepository.UpdateCustomerByIdAsync(customerId, MapCustomerRequestToCustomer(updatedCustomer));

            if (customer != null)
            {
                return MapCustomerToCustomerResponse(customer);
            }

            return null;
        }

        public async Task<CustomerResponse?> DeleteCustomerByIdAsync(int customerId)
        {
            var customer = await _customerRepository.DeleteCustomerByIdAsync(customerId);

            if (customer != null)
            {
                return MapCustomerToCustomerResponse(customer);
            }

            return null;
        }
    }
}
