namespace KWRWebShopAPI.Repositories
{

    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> GetRandomProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product newProduct);
        Task<Product?> UpdateProductAsync(int id, Product updateProduct);
        Task<Product?> DeleteProductAsync(int id);
    }

    public class ProductRepository : IProductRepository
    {


        private readonly DatabaseContext _context;

        public ProductRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Product.Include(c => c.Category).ToListAsync();
        }

        public async Task<List<Product>> GetRandomProductsAsync()
        {
            //return await _context.Product.OrderBy(r => EF.Functions.Random()).Take(3).Include(c => c.Category).ToListAsync();
            return await _context.Product.OrderBy(r => Guid.NewGuid()).Take(3).Include(c => c.Category).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Product.Include(c => c.Category).FirstOrDefaultAsync(x => x.ProductId == id);
        }


        public async Task<Product> CreateProductAsync(Product newProduct)
        {
            _context.Product.Add(newProduct);
            await _context.SaveChangesAsync();
            newProduct = await GetProductByIdAsync(newProduct.ProductId);
            return newProduct;
        }

        public async Task<Product?> DeleteProductAsync(int id)
        {
            var product = await GetProductByIdAsync(id);

            if(product != null)
            {
                _context.Remove(product);
                await _context.SaveChangesAsync();
            }
            return product;
        }



        public async Task<Product?> UpdateProductAsync(int id, Product updateProduct)
        {
            var product = await GetProductByIdAsync(id);

            if(product != null)
            {
                product.Name= updateProduct.Name;
                product.Brand= updateProduct.Brand;
                product.Description= updateProduct.Description;
                product.Price= updateProduct.Price;
                product.CategoryId= updateProduct.CategoryId;
                await _context.SaveChangesAsync();
                product = await GetProductByIdAsync(product.ProductId);
            }

            return product;
        }
    }
}
