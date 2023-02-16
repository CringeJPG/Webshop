using KWRWebShopAPI.Controllers;
using KWRWebShopAPI.DTOs;
using KWRWebShopAPI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWRWebShopTests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly CategoryController _categoryController;
        private readonly Mock<ICategoryService> _categoryServiceMock = new();

        public CategoryControllerTests()
        {
            _categoryController = new(_categoryServiceMock.Object);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenCategoriesExists()
        {
            // Arrange
            List<CategoryResponse> categories = new();

            categories.Add(new CategoryResponse()
            {
                CategoryId = 1,
                CategoryName = "Våben"
            });

            categories.Add(new CategoryResponse()
            {
                CategoryId = 2,
                CategoryName = "VåbenTilbehør"
            });

            _categoryServiceMock
                .Setup(x => x.GetAllCategory())
                .ReturnsAsync(categories);

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.GetAllCategories();

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode204_WhenNoCategoriesExist()
        {
            // Arrange
            List<CategoryResponse> categories = new();

            _categoryServiceMock
                .Setup(x => x.GetAllCategory())
                .ReturnsAsync(categories);

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.GetAllCategories();

            // Assert
            Assert.Equal(204, result.StatusCode);
        }
        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionRaised()
        {
            // Arrange
            List<CategoryResponse> categories = new();

            _categoryServiceMock
                .Setup(x => x.GetAllCategory())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.GetAllCategories();

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void GetCategoryByIdAsync_ShouldReturnStatusCode200_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;

            CategoryResponse categoryResponse = new()
            {
                CategoryId = categoryId,
                CategoryName = "Våben"
            };

            _categoryServiceMock
                .Setup(x => x.GetCategoryById(It.IsAny<int>()))
                .ReturnsAsync(categoryResponse);

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.GetCategoryById(categoryId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }
        [Fact]
        public async void GetCategoryByIdAsync_ShouldReturnStatusCode404_WhenCategoryDoesNotExists()
        {
            // Arrange
            int categoryId = 1;

            _categoryServiceMock
                .Setup(x => x.GetCategoryById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.GetCategoryById(categoryId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }
        [Fact]
        public async void GetCategoryByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange

            _categoryServiceMock
                .Setup(x => x.GetCategoryById(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.GetCategoryById(1);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }
        [Fact]
        public async void CreateCategoryAsync_ShouldReturnStatusCode200_WhenCategoryIsSuccessfullyCreated()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Våben"
            };


            CategoryResponse categoryResponse = new()
            {
                CategoryId = 1,
                CategoryName = "Våben"
            };

            _categoryServiceMock
                .Setup(x => x.CreateCategory(It.IsAny<CategoryRequest>()))
                .ReturnsAsync(categoryResponse);

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.CreateCategory(categoryRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);

        }
        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Våben"
            };

            _categoryServiceMock
                .Setup(x => x.CreateCategory(It.IsAny<CategoryRequest>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.CreateCategory(categoryRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);

        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenCategoryIsUpdated()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Våben"
            };

            int categoryId = 1;

            CategoryResponse categoryResponse = new()
            {
                CategoryId = 1,
                CategoryName = "Våben"
            };

            _categoryServiceMock
                .Setup(x => x.UpdateCategory(It.IsAny<int>(), It.IsAny<CategoryRequest>()))
                .ReturnsAsync(categoryResponse);

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.UpdateCategory(categoryId, categoryRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);

        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenCategoryDoesNotExist()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Våben"
            };
            int categoryId = 1;

            _categoryServiceMock
                .Setup(x => x.UpdateCategory(It.IsAny<int>(), It.IsAny<CategoryRequest>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.UpdateCategory(categoryId, categoryRequest);

            // Assert
            Assert.Equal(404, result.StatusCode);

        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Våben"
            };
            int categoryId = 1;

            _categoryServiceMock
                .Setup(x => x.UpdateCategory(It.IsAny<int>(), It.IsAny<CategoryRequest>()))
                .ReturnsAsync(() => throw new Exception("There has been an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.UpdateCategory(categoryId, categoryRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);

        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenDeleteSuccess()
        {
            // Arrange
            CategoryResponse categoryResponse = new()
            {
                CategoryId = 1,
                CategoryName = "Våben"
            };

            _categoryServiceMock
                .Setup(x => x.DeleteCategory(It.IsAny<int>()))
                .ReturnsAsync(categoryResponse);

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.DeleteCategory(1);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenCategoryDoesNotExists()
        {
            // Arrange

            _categoryServiceMock
                .Setup(x => x.DeleteCategory(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.DeleteCategory(1);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_SHouldReturnStatusCode500_WhenExceptionIsRaised()
        {

            // Arrange
            _categoryServiceMock
                .Setup(x => x.DeleteCategory(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("There is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _categoryController.DeleteCategory(1);

            // Assert
            Assert.Equal(500, result.StatusCode);

        }



    }
}
