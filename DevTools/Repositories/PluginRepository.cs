using DevTools.data;
using DevTools.Dto.Plugins;
using DevTools.Exceptions.AccountManager.RoleException;
using DevTools.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.data;

namespace DevTools.Repositories
{
    public class PluginRepository : IPluginRepository
    {
        private readonly MyDbContext _context;
        private readonly IPluginCategoryRepository _plugincategoryrepository;
        private readonly IPluginManagerRepository _pluginmanagerRepository;
        private readonly IRoleRepository _rolerepository;

        public PluginRepository(MyDbContext _context,
        IPluginCategoryRepository _plugincategoryrepository,
        IRoleRepository _rolerepository,
        IPluginManagerRepository _pluginmanagerRepository)
        {
            this._context = _context;
            this._plugincategoryrepository = _plugincategoryrepository;
            this._rolerepository = _rolerepository;
            this._pluginmanagerRepository = _pluginmanagerRepository;
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
            if (await _plugincategoryrepository.CheckExistByNameAsync(createplugindto.Category) == false)
            {
                await _plugincategoryrepository.AddPluginCategoryAsync(createplugindto.Category);
            }
            var Categoryid = await _plugincategoryrepository.GetIdByName(createplugindto.Category);
            var roleid = await _rolerepository.GetIdByName(createplugindto.AccessiableRole.ToString());

            var newplugin = new Plugin
            {
                Name = createplugindto.Name,
                CategoryId = Categoryid,
                Description = createplugindto.Description,
                IsActive = createplugindto.IsActive,
                IsPremiumTool = createplugindto.IsPremiumTool,
                AccessiableRoleId = roleid
            };
            await _context.Plugins.AddAsync(newplugin);
            await _context.SaveChangesAsync();
            Console.WriteLine($"    ---Added plugin : {createplugindto.Name} into database");
        }
        public async Task<List<Plugin>> GetAllAsync()
        {
            return await _context.Plugins.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<List<Plugin>> GetAllByCategoryAsync(int CategoryId)
        {
            return await _context.Plugins.Where(p => p.CategoryId == CategoryId && p.IsActive == true).ToListAsync();
        }


        public async Task RemovePluginAsync(int pluginId)
        {
            var item = await GetByIdAsync(pluginId);
            if (item == null)
                return;

            _context.Plugins.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetIdByName(string name)
        {
            var item = await _context.Plugins.FirstOrDefaultAsync(x => x.Name == name);
            if (item != null)
                return item.Id;

            return -1;
        }

        public async Task<Plugin> GetByIdAsync(int Id)
        {
            return await _context.Plugins.FindAsync(Id);
        }

        public async Task CheckPluginExistInFoulder()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("Check between Database and foulder tools");
            var plugins_id = await _context.Plugins.Select(x => x.Id).ToListAsync();
            foreach (var plugin_id in plugins_id)
            {
                
                if (await _pluginmanagerRepository.CheckExisted(plugin_id) == false)
                {
                    await RemovePluginAsync(plugin_id);
                }
            }
            Console.WriteLine("==============================================");
        }


    }
}