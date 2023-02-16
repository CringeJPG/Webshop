using KWRWebShopAPI.Controllers;
using KWRWebShopAPI.DTOs;
using KWRWebShopAPI.Repositories;
using KWRWebShopAPI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWRWebShopTests.Controllers
{
    public class ProductControllerTests
    {
        public class HeroControllerTests
        {
            private readonly ProductController _productController;
            private readonly Mock<IProductService> _productServiceMock = new();

            public HeroControllerTests()
            {
                _productController = new(_productServiceMock.Object);
            }
            [Fact]
            public async void GetAllProductsAsync_ShouldReturnStatusCode200_WhenProductsExists()
            {
                // Arrange
                List<ProductResponse> products = new();

                products.Add(new ProductResponse()
                {
                    ProductId = 1,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                });

                products.Add(new ProductResponse()
                {
                    ProductId = 2,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                });

                _productServiceMock
                    .Setup(x => x.GetAllProductsAsync())
                    .ReturnsAsync(products);

                // Act
                var result = (IStatusCodeActionResult)await _productController.GetAllProductsAsync();

                // Assert
                Assert.Equal(200, result.StatusCode);
            }
            [Fact]
            public async void GetAllProductsAsync_ShouldReturnStatusCode204_WhenNoProductsExist()
            {
                // Arrange
                List<ProductResponse> products = new();

                _productServiceMock
                    .Setup(x => x.GetAllProductsAsync())
                    .ReturnsAsync(products);

                // Act
                var result = (IStatusCodeActionResult)await _productController.GetAllProductsAsync();

                // Assert
                Assert.Equal(204, result.StatusCode);
            }
            [Fact]
            public async void GetAllProductsAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
            {
                // Arrange
                List<ProductRepository> products = new();

                _productServiceMock
                    .Setup(x => x.GetAllProductsAsync())
                    .ReturnsAsync(() => throw new Exception("This is an exception"));

                // Act
                var result = (IStatusCodeActionResult)await _productController.GetAllProductsAsync();

                // Assert
                Assert.Equal(500, result.StatusCode);
            }
            [Fact]
            public async void GetRandomProductsAsync_ShouldReturnStatusCode200_WhenProductsExists()
            {
                // Arrange
                List<ProductResponse> products = new();

                products.Add(new ProductResponse()
                {
                    ProductId = 1,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                });

                products.Add(new ProductResponse()
                {
                    ProductId = 2,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                });

                products.Add(new ProductResponse()
                {
                    ProductId = 3,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                });

                _productServiceMock
                    .Setup(x => x.GetRandomProductsAsync())
                    .ReturnsAsync(products);

                // Act
                var result = (IStatusCodeActionResult)await _productController.GetRandomProductsAsync();

                // Assert
                Assert.Equal(200, result.StatusCode);
            }
            [Fact]
            public async void GetRandomProductsAsync_ShouldReturnStatusCode204_WhenNoProductsExist()
            {
                // Arrange
                List<ProductResponse> products = new();

                _productServiceMock
                    .Setup(x => x.GetRandomProductsAsync())
                    .ReturnsAsync(products);

                // Act
                var result = (IStatusCodeActionResult)await _productController.GetRandomProductsAsync();

                // Assert
                Assert.Equal(204, result.StatusCode);
            }
            [Fact]
            public async void GetRandomProductsAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
            {
                // Arrange
                List<ProductRepository> products = new();

                _productServiceMock
                    .Setup(x => x.GetRandomProductsAsync())
                    .ReturnsAsync(() => throw new Exception("This is an exception"));

                // Act
                var result = (IStatusCodeActionResult)await _productController.GetRandomProductsAsync();

                // Assert
                Assert.Equal(500, result.StatusCode);
            }
            [Fact]
            public async void GetProductByIdAsync_ShouldReturnStatusCode200_WhenProductExists()
            {
                // Arrange
                int productId = 1;

                ProductResponse productResponse = new()
                {
                    ProductId = productId,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                };

                _productServiceMock
                    .Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(productResponse);

                // Act
                var result = (IStatusCodeActionResult)await _productController.GetProductByIdAsync(productId);

                // Assert
                Assert.Equal(200, result.StatusCode);
            }
            [Fact]
            public async void GetProductByIdAsync_ShouldReturnStatusCode404_WhenProductDoesNotExists()
            {
                // Arrange
                int productId = 1;

                _productServiceMock
                    .Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(() => null);

                // Act
                var result = (IStatusCodeActionResult)await _productController.GetProductByIdAsync(productId);

                // Assert
                Assert.Equal(404, result.StatusCode);
            }
            [Fact]
            public async void GetProductByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
            {
                // Arrange
                int productId = 1;

                _productServiceMock
                    .Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(() => throw new Exception("This is an exception"));

                // Act
                var result = (IStatusCodeActionResult)await _productController.GetProductByIdAsync(productId);

                // Assert
                Assert.Equal(500, result.StatusCode);
            }
            [Fact]
            public async void CreateProductAsync_ShouldReturnStatusCode200_WhenHeroIsSuccessfullyCreated()
            {
                // Arrange
                ProductRequest productRequest = new()
                {
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                };


                ProductResponse productResponse = new()
                {
                    ProductId = 1,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                };

                _productServiceMock
                    .Setup(x => x.CreateProductAsync(It.IsAny<ProductRequest>()))
                    .ReturnsAsync(productResponse);

                // Act
                var result = (IStatusCodeActionResult)await _productController.CreateProductAsync(productRequest);

                // Assert
                Assert.Equal(200, result.StatusCode);

            }
            [Fact]
            public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
            {
                // Arrange
                ProductRequest productRequest = new()
                {
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                };

                _productServiceMock
                    .Setup(x => x.CreateProductAsync(It.IsAny<ProductRequest>()))
                    .ReturnsAsync(() => throw new Exception("This is an exception"));

                // Act
                var result = (IStatusCodeActionResult)await _productController.CreateProductAsync(productRequest);

                // Assert
                Assert.Equal(500, result.StatusCode);

            }

            [Fact]
            public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenProductIsUpdated()
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

                ProductResponse productResponse = new()
                {
                    ProductId = 1,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                };

                _productServiceMock
                    .Setup(x => x.UpdateProductAsync(It.IsAny<int>(), It.IsAny<ProductRequest>()))
                    .ReturnsAsync(productResponse);

                // Act
                var result = (IStatusCodeActionResult)await _productController.UpdateProductAsync(productId, productRequest);

                // Assert
                Assert.Equal(200, result.StatusCode);

            }

            [Fact]
            public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenProductDoesNotExist()
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

                _productServiceMock
                    .Setup(x => x.UpdateProductAsync(It.IsAny<int>(), It.IsAny<ProductRequest>()))
                    .ReturnsAsync(() => null);

                // Act
                var result = (IStatusCodeActionResult)await _productController.UpdateProductAsync(productId, productRequest);

                // Assert
                Assert.Equal(404, result.StatusCode);

            }
            [Fact]
            public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
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

                _productServiceMock
                    .Setup(x => x.UpdateProductAsync(It.IsAny<int>(), It.IsAny<ProductRequest>()))
                    .ReturnsAsync(() => throw new Exception("There has been an exception"));

                // Act
                var result = (IStatusCodeActionResult)await _productController.UpdateProductAsync(productId, productRequest);

                // Assert
                Assert.Equal(500, result.StatusCode);

            }

            [Fact]
            public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenDeleteSuccess()
            {
                // Arrange
                int productId = 1;

                ProductResponse productResponse = new()
                {
                    ProductId = productId,
                    Name = "Glock 18",
                    Brand = "Toyko Marui",
                    Description = "Airsoft pistol md BlowBack.",
                    Price = 899
                };

                _productServiceMock
                    .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(productResponse);

                // Act
                var result = (IStatusCodeActionResult)await _productController.DeleteProductAsync(productId);

                // Assert
                Assert.Equal(200, result.StatusCode);
            }

            [Fact]
            public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenProductDoesNotExists()
            {
                // Arrange

                _productServiceMock
                    .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(() => null);

                // Act
                var result = (IStatusCodeActionResult)await _productController.DeleteProductAsync(1);

                // Assert
                Assert.Equal(404, result.StatusCode);
            }

            [Fact]
            public async void DeleteByIdAsync_SHouldReturnStatusCode500_WhenExceptionIsRaised()
            {

                // Arrange
                _productServiceMock
                    .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(() => throw new Exception("There is an exception"));

                // Act
                var result = (IStatusCodeActionResult)await _productController.DeleteProductAsync(1);

                // Assert
                Assert.Equal(500, result.StatusCode);

            }

        }
    }
}
