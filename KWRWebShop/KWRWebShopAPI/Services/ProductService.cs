using KWRWebShopAPI.DTOs;
using KWRWebShopAPI.Repositories;

namespace KWRWebShopAPI.Services
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAllProductsAsync();
        Task<List<ProductResponse>> GetRandomProductsAsync();
        Task<ProductResponse?> GetProductByIdAsync(int Id);
        Task<ProductResponse> CreateProductAsync(ProductRequest newProduct);
        Task<ProductResponse?> UpdateProductAsync(int productId, ProductRequest updateProduct);
        Task<ProductResponse?> DeleteByIdAsync(int Id);

    }

    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            List<Product> products = await _productRepository.GetAllProductsAsync();

            if(products == null)
            {
                throw new ArgumentNullException();
            }
            
            return products.Select(product => MapProductToProductResponse(product)).ToList();
        }
        public async Task<List<ProductResponse>> GetRandomProductsAsync()
        {
            List<Product> products = await _productRepository.GetRandomProductsAsync();

            if (products == null)
            {
                throw new ArgumentNullException();
            }

            return products.Select(product => MapProductToProductResponse(product)).ToList();
        }


        private ProductResponse MapProductToProductResponse(Product product)
        {
            return new ProductResponse
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Brand = product.Brand,
                Description = product.Description,
                Price = product.Price,
                Category = new ProductCategoryResponse
                {
                    CategoryId = product.Category.CategoryId,
                    CategoryName = product.Category.CategoryName
                }
            };
        }

        private Product MapProductRequestToProduct(ProductRequest productRequest)
        {
            return new Product
            {
                Name = productRequest.Name,
                Brand = productRequest.Brand,
                Description = productRequest.Description,
                Price = productRequest.Price,
                CategoryId = productRequest.CategoryId
            };
        }

        public async Task<ProductResponse> CreateProductAsync(ProductRequest newProduct)
        {
            var product = await _productRepository.CreateProductAsync(MapProductRequestToProduct(newProduct));

            if(product == null)
            {
                throw new ArgumentNullException();
            }

            return MapProductToProductResponse(product);
        }

        public async Task<ProductResponse?> DeleteByIdAsync(int Id)
        {
            var product = await _productRepository.DeleteProductAsync(Id);

            if(product == null)
            {

                return null;
            }

            return MapProductToProductResponse(product);
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int Id)
        {
            var product = await _productRepository.GetProductByIdAsync(Id);

            if(product == null)
            {
                return null;
            }
            return MapProductToProductResponse(product);
        }

        public async Task<ProductResponse?> UpdateProductAsync(int productId, ProductRequest updateProduct)
        {
            var product = await _productRepository.UpdateProductAsync(productId, MapProductRequestToProduct(updateProduct));

            if (product != null)
            {
                return MapProductToProductResponse(product);
            }

            return null;
        }
    }
}
