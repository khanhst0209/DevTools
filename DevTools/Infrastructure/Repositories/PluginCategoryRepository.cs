using System.Threading.Tasks;
using DevTool.Categories;
using DevTools.data;
using DevTools.Exceptions.Plugins.PluginCategoryException.cs;
using DevTools.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.data;

namespace DevTools.Repositories
{
    public class PluginCategoryRepository : BaseRepository<PluginCategory, int>, IPluginCategoryRepository

    {
        private readonly MyDbContext _context;
        public PluginCategoryRepository(MyDbContext _context) : base(_context)
        {
            this._context = _context;
        }

        public async Task<PluginCategory> GetByName(string name)
        {
            var category = await _context.PluginCategories.FirstOrDefaultAsync(x => x.Name == name);

            return category;
        }

        public override async Task<List<PluginCategory>> GetAllAsync()
        {
            return await _context.PluginCategories.AsNoTracking().Include(p => p.Plugins.Where(x => x.IsActive == true)).Where(x => x.Plugins.Count() > 0).ToListAsync();
        }


        public async Task<int> GetIdByName(string name)
        {
            var item = await _context.PluginCategories.FirstOrDefaultAsync(x => x.Name == name);
            if (item != null)
                return item.Id;

            throw new PluginCategoryNotFound(name);
        }

        public override async Task<PluginCategory> GetByIdAsync(int Id)
        {
            return await (_context.PluginCategories
                                 .Include(p => p.Plugins)
                                 .FirstOrDefaultAsync(x => x.Id == Id));
        }

    }
}