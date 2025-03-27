using AutoMapper;
using DevTools.data;
using DevTools.Dto.Plugins;
using DevTools.Dto.Querry;
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
        private readonly IMapper _mapper;

        public PluginRepository(MyDbContext context,
            IPluginCategoryRepository plugincategoryrepository,
            IRoleRepository rolerepository,
            IPluginManagerRepository pluginmanagerRepository,
            IMapper mapper)
        {
            _context = context;
            _plugincategoryrepository = plugincategoryrepository;
            _rolerepository = rolerepository;
            _pluginmanagerRepository = pluginmanagerRepository;
            _mapper = mapper;
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
                if (existplugin.IsPremium == IsPremium)
                    return;

                existplugin.IsPremium = IsPremium;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddPluginAsync(CreatePluginDTO createplugindto)
        {
            if (!await _plugincategoryrepository.CheckExistByNameAsync(createplugindto.Category))
            {
                await _plugincategoryrepository.AddPluginCategoryAsync(createplugindto.Category);
            }

            var categoryId = await _plugincategoryrepository.GetIdByName(createplugindto.Category);
            var roleId = await _rolerepository.GetIdByName(createplugindto.AccessiableRole.ToString());

            var newPlugin = _mapper.Map<Plugin>(createplugindto);
            newPlugin.CategoryId = categoryId;
            newPlugin.AccessiableRoleId = roleId;

            await _context.Plugins.AddAsync(newPlugin);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Plugin>> GetAllAsync()
        {
            return await _context.Plugins.Where(x => x.IsActive == true).Include(p => p.category).AsNoTracking().ToListAsync();
        }

        public async Task<List<Plugin>> GetAllByCategoryAsync(int CategoryId)
        {
            return await _context.Plugins.Where(p => p.CategoryId == CategoryId && p.IsActive == true).Include(p => p.category).AsNoTracking().ToListAsync();
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

        public async Task<List<Plugin>> GetAllByQuerryAsync(PluginQuerry querry)
        {
            var plugins = _context.Plugins.Where(x => x.IsActive == true).Include(x => x.category).AsQueryable().AsNoTracking();

            if (querry.CategoryId != null)
            {
                plugins = plugins.Where(x => x.CategoryId == querry.CategoryId);
            }
            if (querry.Premium != null && querry.Premium == true)
            {
                plugins = plugins.Where(x => x.IsPremium == true);
            }
            if (querry.Name != null)
            {
                plugins = plugins.Where(x => EF.Functions.Like(x.Name, $"%{querry.Name}%"));
            }

            if (querry.SortBy != null)
            {
                if (querry.SortBy == "Name")
                {
                    plugins = querry.IsDescending ? plugins.OrderByDescending(x => x.Name) : plugins.OrderBy(x => x.Name);
                }
            }

            return await plugins.ToListAsync();
        }

    }
}