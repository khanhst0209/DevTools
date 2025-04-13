using System.Reflection;
using AutoMapper;
using DevTools.Application.Services.AssemblyManager;
using DevTools.Dto.Plugins;
using DevTools.Repositories.Interfaces;
using DevTools.Services.Interfaces;
using Plugins.DevTool;
using Services.AssemblyManager;

namespace DevTools.Services
{
    public class PluginLoader : IPluginLoader
    {
        private readonly IPluginManagerRepository _pluginmanagerRepository;
        private readonly IPluginRepository _pluginRepository;
        private readonly IMapper _mapper;
        private readonly IAssemblyManager _assembleManager;
        private readonly IConfiguration _configuration;
        private readonly string _pluginFolder = "";

        public PluginLoader(IPluginManagerRepository _pluginmanagerRepository
                            , IPluginRepository _pluginRepository,
                             IMapper _mapper,
                             IAssemblyManager _assembleManager,
                             IConfiguration configuration
                             )
        {
            _configuration = configuration;
            _pluginFolder = _configuration["Resources_Path:PluginPath"];

            if (_pluginFolder != null)
                if (!Directory.Exists(_pluginFolder))
                    Directory.CreateDirectory(_pluginFolder);

            this._pluginmanagerRepository = _pluginmanagerRepository;
            this._pluginRepository = _pluginRepository;
            this._mapper = _mapper;
            this._assembleManager = _assembleManager;
        }

        public async Task LoadPluginsAsync()
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

        public async Task AddPluginAsync(string path)
        {
            await LoadPluginFromFile(path);
        }

        public async Task<bool> LoadPluginFromFile(string dllPath)
        {
            try
            {
                var assembly = await _assembleManager.LoadAssemblyAsync(dllPath);
                Type[] types = assembly.GetTypes();


                var pluginTypes = types.Where(t => typeof(IDevToolPlugin).IsAssignableFrom(t) && !t.IsInterface);
                Console.WriteLine($"Found {pluginTypes.Count()} plugin types in {dllPath}");


                foreach (var type in pluginTypes)
                {
                    if (Activator.CreateInstance(type) is IDevToolPlugin plugin)
                    {
                        var item = await _pluginRepository.GetByName(plugin.Name);
                        if (item == null)
                        {
                            var temp = _mapper.Map<CreatePluginDTO>(plugin);
                            temp.DllPath = dllPath;
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
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to load plugin {dllPath}: {ex.Message}");
                return false;
            }
        }

        public async Task RemovePluginAsync(string path)
        {
            try
            {
                Assembly assembly = await _assembleManager.LoadAssemblyAsync(path);
                Type[] types = assembly.GetTypes();
                var pluginTypes = types.Where(t => typeof(IDevToolPlugin).IsAssignableFrom(t) && !t.IsInterface);
                foreach (var type in pluginTypes)
                {
                    if (Activator.CreateInstance(type) is IDevToolPlugin plugin)
                    {
                        var item = await _pluginRepository.GetByName(plugin.Name);
                        if (item == null)
                            return;

                        await _pluginRepository.RemoveAsync(item.Id);
                        await _pluginmanagerRepository.RemoveAsync(item.Id);
                    }
                }
            }
            catch (Exception ex) { }
        }

        public async Task ReplacePluginAsync(string path)
        {
            try
            {
                Assembly assembly = await _assembleManager.LoadAssemblyAsync(path);
                Type[] types = assembly.GetTypes();
                var pluginTypes = types.Where(t => typeof(IDevToolPlugin).IsAssignableFrom(t) && !t.IsInterface);
                foreach (var type in pluginTypes)
                {
                    if (Activator.CreateInstance(type) is IDevToolPlugin plugin)
                    {
                        var item = await _pluginRepository.GetByName(plugin.Name);
                        if (item == null)
                            return;

                        plugin.Id = item.Id;
                        if (item.Icon != plugin.Icon)
                            item.Icon = plugin.Icon;

                        if (item.Description != plugin.Description)
                            item.Description = plugin.Description;


                        await _pluginRepository.UpdateAsync(item);
                        await _pluginmanagerRepository.RemoveAsync(item.Id);
                        await _pluginmanagerRepository.AddAsync(plugin);
                    }
                }
            }
            catch (Exception ex) { }
        }

    }
}