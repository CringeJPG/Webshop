using System.Data;

namespace KWRWebShopAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllCustomerAsync();
        Task<Customer> FindCustomerByIdAsync(int id);
        Task<Customer> CreateCustomerAsync(Customer newCustomer);
        Task<Customer?> UpdateCustomerByIdAsync(int customerId, Customer updatedCustomer);
        Task<Customer?> DeleteCustomerByIdAsync(int customerId);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _context;

        public CustomerRepository(DatabaseContext context)
        {
            _context = context;
        }


        public async Task<List<Customer>> GetAllCustomerAsync()
        {
            return await _context.Customer.Include(l => l.Login).ToListAsync(); // Returnere en liste af costumers som indeholder login, da der er en navigation
                                                                                // property til login entiteten fra customer entiteten
        }

        public async Task<Customer> FindCustomerByIdAsync(int id)
        {
            return await _context.Customer.Include(l => l.Login).FirstOrDefaultAsync(x => x.CustomerId == id);
        }

        public async Task<Customer> CreateCustomerAsync(Customer newCustomer)
        {
            if (_context.Customer.Any(u => u.Login.Email == newCustomer.Login.Email))
            {
                throw new Exception(String.Format("The email {0} is not available", newCustomer.Login.Email));
            }

            _context.Customer.Add(newCustomer);
            await _context.SaveChangesAsync();
            newCustomer = await FindCustomerByIdAsync(newCustomer.CustomerId);
            return newCustomer;
        }

        public async Task<Customer?> UpdateCustomerByIdAsync(int customerId, Customer updatedCustomer)
        {
            var customer = await FindCustomerByIdAsync(customerId);

            if (customer != null)
            {
                customer.FirstName = updatedCustomer.FirstName;
                customer.LastName = updatedCustomer.LastName;
                customer.Address = updatedCustomer.Address;
                customer.Login.Email = updatedCustomer.Login.Email;
                customer.Login.Password = updatedCustomer.Login.Password;
                
                await _context.SaveChangesAsync();
            }
            return customer;
        }

        public async Task<Customer?> DeleteCustomerByIdAsync(int customerId)
        {
            var customer = await FindCustomerByIdAsync(customerId);

            if (customer != null)
            {
                _context.Remove(customer);
                 await _context.SaveChangesAsync();
            }

            return customer;
        }
    }
}
