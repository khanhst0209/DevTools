using System.Reflection;
using AutoMapper;
using DevTools.Dto.Category;
using DevTools.Dto.Plugins;
using DevTools.Dto.Querry;
using DevTools.Exceptions.Plugins.PluginsException.cs;
using DevTools.Repositories.Interfaces;
using DevTools.Services.Interfaces;
using Plugins.DevTool;

namespace DevTools.Services
{
    public class PluginManagerService : IPluginManagerService
    {
        private readonly IPluginManagerRepository _pluginmanagerRepository;
        private readonly IPluginRepository _pluginRepository;
        private readonly IMapper _mapper;
        private readonly string _pluginFolder = "./Plugins/DevTool_Plugins";

        public PluginManagerService(
            IPluginManagerRepository pluginmanagerRepository,
            IPluginRepository pluginRepository,
            IMapper mapper)
        {
            if (!Directory.Exists(_pluginFolder))
                Directory.CreateDirectory(_pluginFolder);

            _pluginmanagerRepository = pluginmanagerRepository;
            _pluginRepository = pluginRepository;
            _mapper = mapper;
        }

        public async Task LoadPlugins()
        {
            Console.WriteLine("================================================");
            Console.WriteLine("Mapping tools from folder into database...");
            await _pluginmanagerRepository.ClearAsync();

            foreach (var dll in Directory.GetFiles(_pluginFolder, "*.dll"))
            {
                Console.WriteLine($"Checking {dll}...");
                await LoadPluginFromFile(dll);
            }
            Console.WriteLine("Check Tool in Database existed in folder");
            await _pluginRepository.CheckPluginExistInFoulder();
            Console.WriteLine("================================================");
        }

        public async Task AddPlugin(string path)
        {
            await LoadPluginFromFile(path);
        }

        private async Task LoadPluginFromFile(string dllPath)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);
                Type[] types = assembly.GetTypes();
                var pluginTypes = types.Where(t => typeof(IDevToolPlugin).IsAssignableFrom(t) && !t.IsInterface);

                foreach (var type in pluginTypes)
                {
                    if (Activator.CreateInstance(type) is IDevToolPlugin plugin)
                    {
                        var item = await _pluginRepository.GetByName(plugin.Name);
                        if (item == null)
                        {
                            var temp = _mapper.Map<CreatePluginDTO>(plugin);
                            await _pluginRepository.AddPluginAsync(temp);
                            plugin.Id = (await _pluginRepository.GetByName(plugin.Name)).Id;
                        }
                        else
                        {
                            plugin.Id = item.Id;
                            if (item.Icon != plugin.Icon)
                                item.Icon = plugin.Icon;

                            if (item.Description != plugin.Description)
                                item.Description = plugin.Description;

                            await _pluginRepository.UpdateAsync(item);
                        }

                        await _pluginmanagerRepository.AddAsync(plugin);
                        Console.WriteLine($"✅ Loaded {dllPath} successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to load plugin {dllPath}: {ex.Message}");
            }
        }

        public async Task<List<PluginsResponeDTO>> GetAllActivePlugin()
        {
            var plugins = await _pluginRepository.GetAllAsync();
            return _mapper.Map<List<PluginsResponeDTO>>(plugins);
        }

        public async Task<object> Execute(int id, object input)
        {
            var plugin = await _pluginmanagerRepository.GetByIdAsync(id);
            return plugin.Execute(input);

        }

        public Task RemovePlugin(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PluginsResponeDTO>> GetAllByQuerry(PluginQuerry querry)
        {
            var plugins = await _pluginRepository.GetAllByQuerryAsync(querry);
            return _mapper.Map<List<PluginsResponeDTO>>(plugins);
        }

        public async Task<string> GetScheme1(int id)
        {
            return await _pluginmanagerRepository.GetScheme1(id);
        }



    }
}
