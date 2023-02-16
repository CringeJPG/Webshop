namespace KWRWebShopAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategory();
        Task<Category?> GetCategoryById(int id);
        Task<Category> CreateCategory(Category newCategory);
        Task<Category?> UpdateCategory(int id, Category updateCategory);
        Task<Category?> DeleteCategory(int id);
    }

    public class CategoryRepository : ICategoryRepository
    {

        private readonly DatabaseContext _context;

        public CategoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategory()
        {
            return await _context.Category.Include(c => c.Products).ToListAsync();

            
        }
        public async Task<Category?> GetCategoryById(int id)
        {
            return await _context.Category.Include(c =>c.Products).SingleOrDefaultAsync(x => x.CategoryId == id);
        }

        public async Task<Category> CreateCategory(Category newCategory)
        {
            _context.Category.Add(newCategory);
            await _context.SaveChangesAsync();
            newCategory = await GetCategoryById(newCategory.CategoryId);
            return newCategory;
        }

        public async Task<Category?> DeleteCategory(int id)
        {
            var category = await GetCategoryById(id);

            if(category != null)
            {
                _context.Remove(category);
                await _context.SaveChangesAsync();
            }
            return category;
        }

        public async Task<Category?> UpdateCategory(int id, Category updateCategory)
        {
            var category = await GetCategoryById(id);

            if (category != null)
            {
                category.CategoryName = updateCategory.CategoryName;
                await _context.SaveChangesAsync();
                category = await GetCategoryById(category.CategoryId);
            }

            return category;
        }
    }
}
