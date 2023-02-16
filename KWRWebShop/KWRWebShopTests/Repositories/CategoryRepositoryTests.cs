using KWRWebShopAPI.Database;
using KWRWebShopAPI.Database.Entities;
using KWRWebShopAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWRWebShopTests.Repositories
{
    public class CategoryRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly CategoryRepository _categoryRepository;

        public CategoryRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "CategoryRepositoryTests")
                .Options;
            _context = new(_options);

            _categoryRepository = new(_context);
        }
        [Fact]
        public async void GetAllCategoriesASync_ShouldReturnListOfCategories_WhenCategoriesExist()
        {
            await _context.Database.EnsureDeletedAsync();

            Product product = new()
            {
                ProductId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899
            };
            Product product2 = new()
            {
                ProductId = 2,
                Name = "AK47",
                Brand = "CYMA",
                Description = "Fuld automatisk assault rifle.",
                Price = 1599
            };

            _context.Add(product);
            _context.Add(product2);

            await _context.SaveChangesAsync();

            _context.Category.Add(new Category
            {
                CategoryId = 1,
                CategoryName = "Våben"
            });

            _context.Category.Add(new Category
            {
                CategoryId = 2,
                CategoryName = "Våbentilbehør"
            });

            await _context.SaveChangesAsync();

            var result = await _categoryRepository.GetAllCategory();

            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public async void GetAllCategoriesAsync_ShouldReturnEmptyListOfCategories_WhenNoCategoriesExist()
        {

            await _context.Database.EnsureDeletedAsync();


            var result = await _categoryRepository.GetAllCategory();


            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void FindCategoryByIdAsync_ShouldReturnCategory_WhenCategoryExists()
        {
            await _context.Database.EnsureDeletedAsync();

            Product product = new()
            {
                ProductId = 1,
                CategoryId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899

            };

            _context.Add(product);
            await _context.SaveChangesAsync();

            _context.Category.Add(new()
            {
                CategoryId = 1,
                CategoryName = "Våben"
            });

            await _context.SaveChangesAsync();

            var result = await _categoryRepository.GetCategoryById(1);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(1, result.CategoryId);
        }
        [Fact]
        public async void FindCategoryByIdAsync_ShouldReturnNull_WhenNoCategoryExists()
        {
            await _context.Database.EnsureDeletedAsync();

            var result = await _categoryRepository.GetCategoryById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void CreateCategoryAsync_ShouldAddNewIdToCategory_WhenSavingToDatabse()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Category category = new()
            {
                CategoryName = "Våben"
            };

            // Act
            var result = await _categoryRepository.CreateCategory(category);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(expectedId, result.CategoryId);
        }
        [Fact]
        public async void CreateCategoryAsync_ShouldFailToAddNewCategory_WhenCategoryIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();


            Category category = new()
            {

                CategoryId = 1,
                CategoryName = "Våben"

            };

            await _categoryRepository.CreateCategory(category);

            // Act
            async Task action() => await _categoryRepository.CreateCategory(category);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnDeletedCategory_WhenCategoryIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Product product = new()
            {
                ProductId = 1,
                CategoryId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899

            };

            _context.Add(product);
            await _context.SaveChangesAsync();

            int categoryId = 1;

            Category category = new()
            {

                CategoryId = categoryId,
                CategoryName = "Våben"
            };

            _context.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.DeleteCategory(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(categoryId, result.CategoryId);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            // Act
            var result = await _categoryRepository.DeleteCategory(1);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async void UpdateProductByIdAsync_ShouldChangeValuesOnProduct_WHenProductExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Product product = new()
            {
                ProductId = 1,
                CategoryId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899

            };

            _context.Add(product);
            await _context.SaveChangesAsync();

            int categoryId = 1;

            Category category = new()
            {
                CategoryId = categoryId,
                CategoryName = "Våben"
            };
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            Category updateProduct = new()
            {
                CategoryId = categoryId,
                CategoryName = "Knives"
            };

            // Act
            var result = await _categoryRepository.UpdateCategory(categoryId, updateProduct);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(categoryId, result?.CategoryId);
            Assert.Equal(updateProduct.CategoryName, result?.CategoryName);
        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Product product = new()
            {
                ProductId = 1,
                CategoryId = 1,
                Name = "Glock 18",
                Brand = "Toyko Marui",
                Description = "Airsoft pistol md BlowBack.",
                Price = 899

            };

            _context.Add(product);
            await _context.SaveChangesAsync();

            int categoryId = 1;
            Category updateCategory = new()
            {
                CategoryId = categoryId,
                CategoryName = "Våben"
            };

            // Act
            var result = await _categoryRepository.UpdateCategory(categoryId, updateCategory);

            // Assert
            Assert.Null(result);
        }
    }
}
