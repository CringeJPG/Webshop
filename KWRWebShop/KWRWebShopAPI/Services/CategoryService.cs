using KWRWebShopAPI.Repositories;
using KWRWebShopAPI.DTOs;
using static KWRWebShopAPI.DTOs.CategoryResponse;

namespace KWRWebShopAPI.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllCategory();
        Task<CategoryResponse?> GetCategoryById(int id);
        Task<CategoryResponse> CreateCategory(CategoryRequest newCategory);
        Task<CategoryResponse?> UpdateCategory(int id, CategoryRequest updateCategory);
        Task<CategoryResponse?> DeleteCategory(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        private CategoryResponse MapCategoryToCategoryResponse(Category category)
        {
            return new CategoryResponse
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Products = category.Products.Select(x => new CategoryProductResponse
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    Brand = x.Brand,
                    Description = x.Description,
                    Price = x.Price
                }).ToList()
            };
        }
        private Category MapCategoryRequestToCategory(CategoryRequest categoryRequest)
        {
            return new Category
            {
                CategoryName = categoryRequest.CategoryName
            };
        }

        public async Task<List<CategoryResponse>> GetAllCategory()
        {
            List<Category> categories = await _categoryRepository.GetAllCategory();

            if (categories == null)
            {
                throw new ArgumentNullException();
            }

            return categories.Select(x => MapCategoryToCategoryResponse(x)).ToList();


        }
        public async Task<CategoryResponse?> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            
            if(category != null)
            {
                return MapCategoryToCategoryResponse(category);
            }

            return null;
        }


        public async Task<CategoryResponse> CreateCategory(CategoryRequest newCategory)
        {
            var category = await _categoryRepository.CreateCategory(MapCategoryRequestToCategory(newCategory));

            if(category != null)
            {
                return MapCategoryToCategoryResponse(category);
            }
            throw new ArgumentNullException();
        }

        public async Task<CategoryResponse?> DeleteCategory(int id)
        {
            var category = await _categoryRepository.DeleteCategory(id);

            if(category != null)
            {
                return MapCategoryToCategoryResponse(category);
            }
            return null;
        }

        public async Task<CategoryResponse?> UpdateCategory(int id, CategoryRequest updateCategory)
        {
            var category = await _categoryRepository.UpdateCategory(id, MapCategoryRequestToCategory(updateCategory));

            if(category != null)
            {
                return MapCategoryToCategoryResponse(category);
            }
            return null;
        }
    }
}
