using KWRWebShopAPI.Database.Entities;
using KWRWebShopAPI.DTOs;
using KWRWebShopAPI.Repositories;
using KWRWebShopAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWRWebShopTests.Services
{
    public class CategoryServiceTests
    {
        private readonly CategoryService _categoryService;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();

        public CategoryServiceTests()
        {
            _categoryService = new(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllCategoryASync_ShouldReturnListOfCategoryResponses_WhenCategoryExist()
        {
            List<Category> categories = new()
            {
                new()
                {
                    CategoryId = 1,
                    CategoryName = "Våben",
                },
                new()
                {

                    CategoryId = 2,
                    CategoryName = "Våbentilbehør"
                    
                }
            };

            _categoryRepositoryMock
                .Setup(x => x.GetAllCategory())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllCategory();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CategoryResponse>>(result);
            Assert.Equal(2, result?.Count);

        }

        [Fact]
        public async void GetAllCategoriesAsync_ShouldReturnEmptyListOfCategoryResponses_WhenNoCategoriesExist()
        {
            List<Category> categories = new();

            _categoryRepositoryMock.Setup(x => x.GetAllCategory()).ReturnsAsync(categories);

            var result = await _categoryService.GetAllCategory();

            Assert.NotNull(result);
            Assert.IsType<List<CategoryResponse>>(result);
            Assert.Empty(result);

        }
        [Fact]
        public async void GetAllCategoriesAsync_ShouldThrowNullExeption_WhenRepositioryReturnsNull()
        {
            // Arrange 
            _categoryRepositoryMock
                .Setup(x => x.GetAllCategory())
                .ReturnsAsync(() => throw new ArgumentNullException());
            // Act
            async Task action() => await _categoryService.GetAllCategory();

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Contains("Value cannot be null", ex.Message);

        }
        [Fact]
        public async void GetCategoryByIdAsync_ShouldReturnCategoryResponse_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;

            Category category = new()
            {
                CategoryId = categoryId,
                CategoryName = "Våben"
            };

            _categoryRepositoryMock
                .Setup(x => x.GetCategoryById(It.IsAny<int>()))
                .ReturnsAsync(category);


            // Act
            var result = await _categoryService.GetCategoryById(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CategoryResponse>(result);
            Assert.Equal(category.CategoryId, result?.CategoryId);
            Assert.Equal(category.CategoryName, result?.CategoryName);

        }
        [Fact]
        public async void CategoryByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange

            _categoryRepositoryMock
                .Setup(x => x.GetCategoryById(It.IsAny<int>()))
                .ReturnsAsync(() => null);


            // Act
            var result = await _categoryService.GetCategoryById(1);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async void CreateAsync_ShouldReturnCategoryResponse_WhenCreateIsSuccess()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Justice League"
            };
            int categoryId = 1;

            Category category = new()
            {
                CategoryId = categoryId,
                CategoryName = "Våben"
            };

            _categoryRepositoryMock
                .Setup(x => x.CreateCategory(It.IsAny<Category>()))
                .ReturnsAsync(category);


            // Act
            var result = await _categoryService.CreateCategory(categoryRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CategoryResponse>(result);
            Assert.Equal(category.CategoryId, result.CategoryId);
            Assert.Equal(category.CategoryName, result.CategoryName);
        }
        [Fact]
        public async void CreateAsync_ShouldThrowNullExeption_WhenRepositioryReturnsNull()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Våben"
            };

            _categoryRepositoryMock
                .Setup(x => x.CreateCategory(It.IsAny<Category>()))
                .ReturnsAsync(() => null);


            // Act
            async Task action() => await _categoryService.CreateCategory(categoryRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Contains("Value cannot be null", ex.Message);
        }
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnCategoryResponse_WhenDeleteIsSuccess()
        {
            // Arrange
            int categoryId = 1;

            Category category = new()
            {
                CategoryId = categoryId,
                CategoryName = "Våben"
            };

            _categoryRepositoryMock
                .Setup(x => x.DeleteCategory(It.IsAny<int>()))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.DeleteCategory(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CategoryResponse>(result);
            Assert.Equal(category.CategoryId, categoryId);

        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenNoCategoryExists()
        {
            // Arrange
            _categoryRepositoryMock
                .Setup(x => x.DeleteCategory(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _categoryService.DeleteCategory(1);

            // Assert
            Assert.Null(result);


        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnCategoryResponse_WhenUpdateSuccess()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Våben"
            };
            int categoryId = 1;
            Category updateCategory = new()
            {
                CategoryId = categoryId,
                CategoryName = "Våben"

            };
            _categoryRepositoryMock
                .Setup(x => x.UpdateCategory(It.IsAny<int>(), It.IsAny<Category>()))
                .ReturnsAsync(updateCategory);


            // Act
            var result = await _categoryService.UpdateCategory(categoryId, categoryRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CategoryResponse>(result);
            Assert.Equal(categoryId, result?.CategoryId);
            Assert.Equal(categoryRequest.CategoryName, result?.CategoryName);
        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Våben"
            };
            int categoryId = 1;
            _categoryRepositoryMock
                .Setup(x => x.UpdateCategory(It.IsAny<int>(), It.IsAny<Category>()))
                .ReturnsAsync(() => null);


            // Act
            var result = await _categoryService.UpdateCategory(categoryId, categoryRequest);

            // Assert
            Assert.Null(result);

        }
    }
}
