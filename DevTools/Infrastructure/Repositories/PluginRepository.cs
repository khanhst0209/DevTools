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
    public class PluginRepository : BaseRepository<Plugin, int>, IPluginRepository
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
            IMapper mapper) : base(context)
        {
            _context = context;
            _plugincategoryrepository = plugincategoryrepository;
            _rolerepository = rolerepository;
            _pluginmanagerRepository = pluginmanagerRepository;
            _mapper = mapper;
        }

        public async Task AddPluginAsync(CreatePluginDTO createplugindto)
        {
            if ((await _plugincategoryrepository.GetByName(createplugindto.Category)) == null)
            {
                var temp = new PluginCategory { Name = createplugindto.Category };
                await _plugincategoryrepository.AddAsync(temp);
            }

            var category = await _plugincategoryrepository.GetByName(createplugindto.Category);
            var roleId = await _rolerepository.GetIdByName(createplugindto.AccessiableRole.ToString());

            var newPlugin = _mapper.Map<Plugin>(createplugindto);
            newPlugin.CategoryId = category.Id;
            newPlugin.AccessiableRoleId = roleId;

            await _context.Plugins.AddAsync(newPlugin);
            await _context.SaveChangesAsync();
        }

        public async Task<Plugin> GetByName(string name)
        {
            var item = await _context.Plugins.FirstOrDefaultAsync(x => x.Name == name);
            return item;
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
                    await RemoveAsync(plugin_id);
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
                plugins = plugins.Where(x => EF.Functions.FreeText(x.Name, querry.Name));
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

        public override async Task<List<Plugin>> GetAllAsync()
        {
            return await _context.Plugins.Include(x => x.category).ToListAsync();
        }

    }
}