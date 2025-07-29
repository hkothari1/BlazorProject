using BlazorFinalProject.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorFinalProject.Repository
{
    public class ProductRepository : IRepository.IProductRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        private IWebHostEnvironment _webHostEnvironment;

        public ProductRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _dbcontext.Products.AddAsync(product);
            await _dbcontext.SaveChangesAsync();
            return product;
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _dbcontext.Products.FirstOrDefaultAsync(c => c.Id == id);

            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('/'));
            if(File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
            if (product == null) return false;

            _dbcontext.Products.Remove(product);
            return (await _dbcontext.SaveChangesAsync()) > 0;
        }

        public async Task<Product> GetAsync(int id)
        {
            var product = await _dbcontext.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (product == null) 
                return new Product();
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbcontext.Products.Include(u=>u.Category).ToListAsync();
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var existingProduct = await _dbcontext.Products.FirstOrDefaultAsync(c => c.Id == product.Id);
            if (existingProduct != null) {

                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.SpecialTag = product.SpecialTag;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;

                _dbcontext.Products.Update(existingProduct);
                await _dbcontext.SaveChangesAsync();
                return existingProduct;

            }
            return product; // Return the original product if not found
        }
    }
}
