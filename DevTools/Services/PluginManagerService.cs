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
                        var id = await _pluginRepository.GetIdByName(plugin.Name);
                        if (id == -1)
                        {
                            var temp = _mapper.Map<CreatePluginDTO>(plugin);
                            await _pluginRepository.AddPluginAsync(temp);
                            plugin.Id = await _pluginRepository.GetIdByName(plugin.Name);
                        }
                        else
                        {
                            plugin.Id = id;
                            Console.WriteLine($"Plugin already exists in database.");
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
            try
            {
                var plugin = await _pluginmanagerRepository.GetByIdAsync(id);
                return plugin.Execute(input);
            }
            catch (PluginNotFound ex)
            {
                throw ex;
            }
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
    }
}
