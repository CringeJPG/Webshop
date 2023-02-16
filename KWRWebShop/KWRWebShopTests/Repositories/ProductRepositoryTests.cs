using Microsoft.EntityFrameworkCore;
using KWRWebShopAPI.Database;
using KWRWebShopAPI.Database.Entities;
using KWRWebShopAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWRWebShopTests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly ProductRepository _productRepository;

        public ProductRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ProductRepositoryTests")
                .Options;
            _context = new(_options);

            _productRepository = new(_context);
        }

        [Fact]
        public async void GetAllProductsASync_ShouldReturnListOfProducts_WhenProductsExist()
        {
            await _context.Database.EnsureDeletedAsync();

            Category category = new()
            {
                CategoryId = 1,
                CategoryName = "Våben"
            };
            Category category2 = new()
            {
                CategoryId = 2,
                CategoryName = "Våbentilbehør"
            };

            _context.Add(category);
            _context.Add(category2);
            await _context.SaveChangesAsync();

            _context.Product.Add(new Product
            {
                ProductId = 1,
                CategoryId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899
            });

            _context.Product.Add(new Product
            {
                ProductId = 2,
                CategoryId = 2,
                Name = "AK47",
                Brand = "CYMA",
                Description = "Fuld automatisk assault rifle.",
                Price = 1599
            });

            await _context.SaveChangesAsync();

            var result = await _productRepository.GetAllProductsAsync();

            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public async void GetAllProductsAsync_ShouldReturnEmptyListOfProducts_WhenNoProductsExist()
        {

            await _context.Database.EnsureDeletedAsync();


            var result = await _productRepository.GetAllProductsAsync();


            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result);
            Assert.Empty(result);
        }
        [Fact]
        public async void GetRandomProductsASync_ShouldReturnListOfProducts_WhenProductsExist()
        {
            await _context.Database.EnsureDeletedAsync();

            Category category = new()
            {
                CategoryId = 1,
                CategoryName = "Våben"
            };
            Category category2 = new()
            {
                CategoryId = 2,
                CategoryName = "Våbentilbehør"
            };

            _context.Add(category);
            _context.Add(category2);
            await _context.SaveChangesAsync();

            _context.Product.Add(new Product
            {
                ProductId = 1,
                CategoryId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899
            });

            _context.Product.Add(new Product
            {
                ProductId = 2,
                CategoryId = 2,
                Name = "AK47",
                Brand = "CYMA",
                Description = "Fuld automatisk assault rifle.",
                Price = 1599
            });

            _context.Product.Add(new Product
            {
                ProductId = 3,
                CategoryId = 2,
                Name = "AK47",
                Brand = "CYMA",
                Description = "Fuld automatisk assault rifle.",
                Price = 1599
            });

            await _context.SaveChangesAsync();

            var result = await _productRepository.GetRandomProductsAsync();

            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result);
            Assert.Equal(3, result.Count);
        }
        [Fact]
        public async void GetRandomProductsAsync_ShouldReturnEmptyListOfProducts_WhenNoProductsExist()
        {

            await _context.Database.EnsureDeletedAsync();


            var result = await _productRepository.GetRandomProductsAsync();


            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void FindProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            await _context.Database.EnsureDeletedAsync();

            Category category = new()
            {
                CategoryId = 1,
                CategoryName = "Våben"
            };

            _context.Add(category);
            await _context.SaveChangesAsync();

            _context.Product.Add(new()
            {
                ProductId = 1,
                CategoryId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899
            });

            await _context.SaveChangesAsync();

            var result = await _productRepository.GetProductByIdAsync(1);

            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(1, result.ProductId);
        }
        [Fact]
        public async void FindProductByIdAsync_ShouldReturnNull_WhenNoProductExists()
        {
            await _context.Database.EnsureDeletedAsync();

            var result = await _productRepository.GetProductByIdAsync(1);

            Assert.Null(result);
        }
        [Fact]
        public async void CreateProductAsync_ShouldAddNewIdTProduct_WhenSavingToDatabse()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Product product = new()
            {
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899,
                Category = new Category()
                {
                    CategoryId = 1,
                    CategoryName = "test"
                }
            };

            // Act
            var result = await _productRepository.CreateProductAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(expectedId, result.ProductId);
        }
        [Fact]
        public async void CreateProductAsync_ShouldFailToAddNewProduct_WhenProductIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();


            Product product = new()
            {

                ProductId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899

            };

            await _productRepository.CreateProductAsync(product);

            // Act
            async Task action() => await _productRepository.CreateProductAsync(product);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }
        [Fact]
        public async void UpdateProductByIdAsync_ShouldChangeValuesOnProduct_WHenProductExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Category category = new()
            {
                CategoryId = 1,
                CategoryName = "Våben"
            };

            _context.Add(category);
            await _context.SaveChangesAsync();

            int productId = 1;

            Product product = new()
            {
                ProductId = productId,
                CategoryId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899
            };
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            Product updateProduct = new()
            {
                ProductId = productId,
                CategoryId = 1,
                Name = "Glock 20",
                Brand = "China Marui",
                Description = "Real gun pistol md BlowBack.",
                Price = 2000
            };

            // Act
            var result = await _productRepository.UpdateProductAsync(productId, updateProduct);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(productId, result?.ProductId);
            Assert.Equal(updateProduct.Name, result?.Name);
            Assert.Equal(updateProduct.Brand, result?.Brand);
            Assert.Equal(updateProduct.Description, result?.Description);
            Assert.Equal(updateProduct.Price, result?.Price);
        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Category category = new()
            {
                CategoryId = 1,
                CategoryName = "Våben"
            };

            _context.Add(category);
            await _context.SaveChangesAsync();

            int productId = 1;
            Product updateProduct = new()
            {
                ProductId = productId,
                CategoryId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899
            };

            // Act
            var result = await _productRepository.UpdateProductAsync(productId, updateProduct);

            // Assert
            Assert.Null(result);

        }
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnDeletedProduct_WhenProductIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Category category = new()
            {
                CategoryId = 1,
                CategoryName = "Våben"
            };

            _context.Add(category);
            await _context.SaveChangesAsync();

            int productId = 1;

            Product product = new()
            {
                ProductId = productId,
                CategoryId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899

            };

            _context.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepository.DeleteProductAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(productId, result.ProductId);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenHeroDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            // Act
            var result = await _productRepository.DeleteProductAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}
