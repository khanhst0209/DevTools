using DevTools.data;
using DevTools.Dto.Plugins;
using DevTools.Exceptions.AccountManager.RoleException;
using DevTools.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.data;

namespace DevTools.Repositories
{
    public class PluginRepository : IPluginRepository
    {
        private readonly MyDbContext _context;
        private readonly IPluginCategoryRepository _plugincategoryrepository;

        private readonly IRoleRepository _rolerepository;

        public PluginRepository(MyDbContext _context,
        IPluginCategoryRepository _plugincategoryrepository,
        IRoleRepository _rolerepository)
        {
            this._context = _context;
            this._plugincategoryrepository = _plugincategoryrepository;
            this._rolerepository = _rolerepository;
        }

        public async Task ActivateAsync(int pluginId, bool Isactivate)
        {
            var existplugin = await _context.Plugins.FindAsync(pluginId);

            if (existplugin != null)
            {
                if (existplugin.IsActive == Isactivate)
                    return;

                existplugin.IsActive = Isactivate;
                await _context.SaveChangesAsync();
            }
        }
        public async Task PremiumAsync(int pluginId, bool IsPremium)
        {
            var existplugin = await _context.Plugins.FindAsync(pluginId);

            if (existplugin != null)
            {
                if (existplugin.IsPremiumTool == IsPremium)
                    return;

                existplugin.IsPremiumTool = IsPremium;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddPluginAsync(CreatePluginDTO createplugindto)
        {
            if (await CheckExistByName(createplugindto.Name) == true)
                return;

            if (await _plugincategoryrepository.CheckExistByNameAsync(createplugindto.Category) == false)
            {
                await _plugincategoryrepository.AddPluginCategoryAsync(createplugindto.Category);
            }

            var newplugin = new Plugin
            {
                Name = createplugindto.Name,
                Categoryid = await _plugincategoryrepository.GetIdByName(createplugindto.Category),
                Description = createplugindto.Description,
                IsActive = createplugindto.IsActive,
                IsPremiumTool = createplugindto.IsPremiumTool,
                AccessiableRole = await _rolerepository.GetIdByName(createplugindto.AccessiableRole.ToString())
            };
        
            await _context.Plugins.AddAsync(newplugin);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Plugin>> GetAllAsync()
        {
            return await _context.Plugins.ToListAsync();
        }

        public async Task<List<Plugin>> GetAllByCategoryAsync(int CategoryId)
        {
            return await _context.Plugins.Where(p => p.Categoryid == CategoryId).ToListAsync();
        }
        

        public async Task RemovePluginAsync(int pluginId)
        {
            var item = await GetByIdAsync(pluginId);
            if(item == null)
                return;
            
            _context.Plugins.Remove(item);
            _context.SaveChangesAsync();
        }

        public async Task<bool> CheckExistByName(string name)
        {
            var item = await _context.Plugins.FirstOrDefaultAsync(x => x.Name == name);
            return item != null;
        }

        public async Task<Plugin> GetByIdAsync(int Id)
        {
            return await _context.Plugins.FindAsync(Id);
        }

    }
}