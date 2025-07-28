using BlazorFinalProject.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorFinalProject.Repository
{
    public class CategoryRepository : IRepository.ICategoryRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public CategoryRepository(ApplicationDbContext context)
        {
            _dbcontext = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _dbcontext.Categories.AddAsync(category);
            await _dbcontext.SaveChangesAsync();
            return category;
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _dbcontext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return false;

            _dbcontext.Categories.Remove(category);
            return (await _dbcontext.SaveChangesAsync()) > 0;
        }

        public async Task<Category> GetAsync(int id)
        {
            var category = await _dbcontext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) 
                return new Category();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbcontext.Categories.ToListAsync();
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var existingCategory = await _dbcontext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (existingCategory != null) {

                existingCategory.Name = category.Name;
                _dbcontext.Categories.Update(existingCategory);
                await _dbcontext.SaveChangesAsync();
                return existingCategory;

            }
            return category; // Return the original category if not found
        }
    }
}
