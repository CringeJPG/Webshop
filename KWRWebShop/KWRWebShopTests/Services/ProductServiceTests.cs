using Moq;
using KWRWebShopAPI.Repositories;
using KWRWebShopAPI.Services;
using KWRWebShopAPI.Database.Entities;
using KWRWebShopAPI.DTOs;

namespace KWRWebShopTests.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        private readonly Mock<IProductRepository> _productRepositoryMock = new();

        public ProductServiceTests()
        {
            _productService = new(_productRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllProductsASync_ShouldReturnListOfProductResponses_WhenProductsExist()
        {
            List<Product> products = new()
            {
                new()
                {
                    ProductId = 1,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899,
                    Category = new()
                    {
                        CategoryId = 1,
                        CategoryName = "Våben"
                    }
                },
                new()
                {
                    ProductId = 2,
                    Name = "AK47",
                    Brand = "CYMA",
                    Description = "Fuld automatisk assault rifle.",
                    Price = 1599,
                    Category = new()
                    {
                        CategoryId = 1,
                        CategoryName = "Våbentilbehør"
                    }
                }
            };

            _productRepositoryMock
                .Setup(x => x.GetAllProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ProductResponse>>(result);
            Assert.Equal(2, result?.Count);

        }

        [Fact]
        public async void GetAllProductsAsync_ShouldReturnEmptyListOfProductResponses_WhenNoProductsExist()
        {
            List<Product> products = new();

            _productRepositoryMock.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(products);

            var result = await _productService.GetAllProductsAsync();

            Assert.NotNull(result);
            Assert.IsType<List<ProductResponse>>(result);
            Assert.Empty(result);

        }
        [Fact]
        public async void GetAllProductsAsync_ShouldThrowNullExeption_WhenRepositioryReturnsNull()
        {
            // Arrange 
            _productRepositoryMock
                .Setup(x => x.GetAllProductsAsync())
                .ReturnsAsync(() => throw new ArgumentNullException());
            // Act
            async Task action() => await _productService.GetAllProductsAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Contains("Value cannot be null", ex.Message);

        }
        [Fact]
        public async void GetRandomProductsASync_ShouldReturnListOfProductResponses_WhenProductsExist()
        {
            List<Product> products = new()
            {
                new()
                {
                    ProductId = 1,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899,
                    Category = new()
                    {
                        CategoryId = 1,
                        CategoryName = "Våben"
                    }
                },
                new()
                {
                    ProductId = 2,
                    Name = "AK47",
                    Brand = "CYMA",
                    Description = "Fuld automatisk assault rifle.",
                    Price = 1599,
                    Category = new()
                    {
                        CategoryId = 1,
                        CategoryName = "Våbentilbehør"
                    }
                },
                new()
                {
                    ProductId = 3,
                    Name = "AK47",
                    Brand = "CYMA",
                    Description = "Fuld automatisk assault rifle.",
                    Price = 1599,
                    Category = new()
                    {
                        CategoryId = 1,
                        CategoryName = "Våbentilbehør"
                    }
                }
            };

            _productRepositoryMock
                .Setup(x => x.GetRandomProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetRandomProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ProductResponse>>(result);
            Assert.Equal(3, result?.Count);

        }

        [Fact]
        public async void GetRandomProductsAsync_ShouldReturnEmptyListOfProductResponses_WhenNoProductsExist()
        {
            List<Product> products = new();

            _productRepositoryMock.Setup(x => x.GetRandomProductsAsync()).ReturnsAsync(products);

            var result = await _productService.GetRandomProductsAsync();

            Assert.NotNull(result);
            Assert.IsType<List<ProductResponse>>(result);
            Assert.Empty(result);

        }
        [Fact]
        public async void GetRandomProductsAsync_ShouldThrowNullExeption_WhenRepositioryReturnsNull()
        {
            // Arrange 
            _productRepositoryMock
                .Setup(x => x.GetRandomProductsAsync())
                .ReturnsAsync(() => throw new ArgumentNullException());
            // Act
            async Task action() => await _productService.GetRandomProductsAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Contains("Value cannot be null", ex.Message);

        }
        [Fact]
        public async void CreateProductAsync_ShouldReturnHeroResponse_WhenCreateIsSuccess()
        {
            // Arrange
            ProductRequest productRequest = new()
            {
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899,
                CategoryId = 1
            };
            int productId = 1;

            Product product = new()
            {
                ProductId = productId,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899,
                Category = new()
                {
                    CategoryId = 1,
                    CategoryName = "Våben"
                }
            };

            _productRepositoryMock
                .Setup(x => x.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(product);


            // Act
            var result = await _productService.CreateProductAsync(productRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result);
            Assert.Equal(product.ProductId, result.ProductId);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Brand, result.Brand);
            Assert.Equal(product.Description, result.Description);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.Category.CategoryId, product.Category.CategoryId);
        }
        [Fact]
        public async void CreateProductAsync_ShouldThrowNullExeption_WhenRepositioryReturnsNull()
        {
            // Arrange
            ProductRequest productRequest = new()
            {
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899
            };

            _productRepositoryMock
                .Setup(x => x.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(() => null);


            // Act
            async Task action() => await _productService.CreateProductAsync(productRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Contains("Value cannot be null", ex.Message);
        }

        [Fact]
        public async void GetProductByIdAsync_ShouldReturnProductResponse_WhenProductsExists()
        {
            // Arrange
            int productId = 1;

            Product product = new()
            {
                ProductId = productId,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899,
                Category = new()
                {
                    CategoryId = 1,
                    CategoryName = "Våben"
                }
            };

            _productRepositoryMock
                .Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);


            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result);
            Assert.Equal(product.ProductId, result?.ProductId);
            Assert.Equal(product.Name, result?.Name);
            Assert.Equal(product.Brand, result?.Brand);
            Assert.Equal(product.Description, result?.Description);
            Assert.Equal(product.Price, result?.Price);
            Assert.Equal(product.Category.CategoryId, result?.Category.CategoryId);
        }
        [Fact]
        public async void GetProductByIdIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange

            _productRepositoryMock
                .Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);


            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateProductAsync_ShouldReturnProductResponse_WhenUpdateSuccess()
        {
            // Arrange
            ProductRequest productRequest = new()
            {
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899,
                CategoryId = 1
            };
            int productId = 1;
            Product updateProduct = new()
            {
                ProductId = productId,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899,
                Category = new()
                {
                    CategoryId = 1,
                    CategoryName = "Våben"
                }
            };
            _productRepositoryMock
                .Setup(x => x.UpdateProductAsync(It.IsAny<int>(), It.IsAny<Product>()))
                .ReturnsAsync(updateProduct);


            // Act
            var result = await _productService.UpdateProductAsync(productId, productRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result);
            Assert.Equal(productId, result?.ProductId);
            Assert.Equal(productRequest.Name, result?.Name);
            Assert.Equal(productRequest.Brand, result?.Brand);
            Assert.Equal(productRequest.Description, result?.Description);
            Assert.Equal(productRequest.Price, result?.Price);
            Assert.Equal(productRequest.CategoryId, result?.Category.CategoryId);
        }
        [Fact]
        public async void UpdateProductAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            ProductRequest productRequest = new()
            {
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899
            };
            int productId = 1;
            _productRepositoryMock
                .Setup(x => x.UpdateProductAsync(It.IsAny<int>(), It.IsAny<Product>()))
                .ReturnsAsync(() => null);


            // Act
            var result = await _productService.UpdateProductAsync(productId, productRequest);

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public async void DeleteProductAsync_ShouldReturnProductResponse_WhenDeleteIsSuccess()
        {
            // Arrange
            int productId = 1;

            Product product = new()
            {
                ProductId = productId,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899,
                Category = new()
                {
                    CategoryId = 1,
                    CategoryName = "Våben"
                }

            };

            _productRepositoryMock
                .Setup(x => x.DeleteProductAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.DeleteByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result);
            Assert.Equal(product.ProductId, productId);

        }

        [Fact]
        public async void DeleteProductAsync_ShouldReturnNull_WhenNoProductExists()
        {
            // Arrange
            _productRepositoryMock
                .Setup(x => x.DeleteProductAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _productService.DeleteByIdAsync(1);

            // Assert
            Assert.Null(result);


        }


    }
}
