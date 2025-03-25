using System.Threading.Tasks;
using DevTools.data;
using DevTools.Exceptions.Plugins.PluginCategoryException.cs;
using DevTools.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.data;

namespace DevTools.Repositories
{
    public class PluginCategoryRepository : IPluginCategoryRepository

    {
        private readonly MyDbContext _context;
        public PluginCategoryRepository(MyDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddPluginCategoryAsync(string name)
        {
            var newcategory = new PluginCategory{
                Name = name
            };
            await _context.PluginCategories.AddAsync(newcategory);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckExistByNameAsync(string name)
        {
            var category = await _context.PluginCategories.FirstOrDefaultAsync(x => x.Name == name);

            return category != null;
        }

        public async Task<List<PluginCategory>> GetAllAsync()
        {
            return await _context.PluginCategories.AsNoTracking().Include(p => p.Plugins).ToListAsync();
        }

        public async Task<PluginCategory> GetByIdAsync(int Id)
        {
            return await _context.PluginCategories.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<int> GetIdByName(string name)
        {
            var item = await _context.PluginCategories.FirstOrDefaultAsync(x => x.Name == name);
            if(item != null)
                return item.Id;
            
            throw new PluginCategoryNotFound(name);
        }


        public async Task RemoveByIdAsync(int Id)
        {
            var existingCategory = await _context.PluginCategories.FindAsync(Id);
            if(existingCategory != null)
            {
                _context.PluginCategories.Remove(existingCategory);
                await _context.SaveChangesAsync();
            }
        }


    }
}