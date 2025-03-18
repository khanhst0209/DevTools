using System.Reflection;
using DevTools.Dto.Plugins;
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

        private readonly string _pluginFolder = "./Plugins/DevTool_Plugins";

        public PluginManagerService(IPluginManagerRepository _pluginmanagerRepository,
        IPluginRepository _pluginRepository,
        IPluginCategoryRepository _plugincategoryrepository)
        {
            if (!Directory.Exists(_pluginFolder))
                Directory.CreateDirectory(_pluginFolder);
            this._pluginRepository = _pluginRepository;
            this._pluginmanagerRepository = _pluginmanagerRepository;
            // this._plugincategoryrepository = _plugincategoryrepository;
        }

        public async Task LoadPlugins()
        {
            Console.WriteLine("================================================");
            Console.WriteLine("Map tool in folder into database");
            await _pluginmanagerRepository.ClearAsync();
            foreach (var dll in Directory.GetFiles(_pluginFolder, "*.dll"))
            {
                Console.WriteLine($"check {dll}");
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
                            var temp = new CreatePluginDTO
                            {
                                Name = plugin.Name,
                                Category = plugin.Category,
                                Description = plugin.Description,
                                AccessiableRole = plugin.AccessiableRole,
                                IsActive = plugin.IsActive,
                                IsPremiumTool = plugin.IsPremiumTool
                            };

                            await _pluginRepository.AddPluginAsync(temp);

                            var Id = await _pluginRepository.GetIdByName(plugin.Name);
                            plugin.id = Id;
                        }
                        else
                        {
                            plugin.id = id;
                            Console.WriteLine($"Plugin already existed in database");
                        }

                        await _pluginmanagerRepository.AddAsync(plugin);
                        Console.WriteLine($"Load {dllPath} Succesfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to load plugin {dllPath}: {ex.Message}");
            }
        }

        public async Task RemovePlugin(string path)
        {
            // try
            // {
            //     Assembly assembly = Assembly.LoadFrom(path);
            //     Type[] types = assembly.GetTypes();
            //     var pluginTypes = types.Where(t => typeof(IDevToolPlugin).IsAssignableFrom(t) && !t.IsInterface);

            //     foreach (var type in pluginTypes)
            //     {
            //         var pluginToRemove = _plugins.FirstOrDefault(p => p.GetType() == type);
            //         if (pluginToRemove != null)
            //         {
            //             _plugins.Remove(pluginToRemove);
            //             Console.WriteLine($"🗑️ Đã xóa plugin: {pluginToRemove.Name}");
            //         }
            //         // _pluginmanagerRepository.RemoveAsync(type);
            //     }
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"❌ Lỗi khi xóa plugin: {ex.Message}");
            // }
            Console.WriteLine("Deo lam cac gi ca, test thoi");
        }

        public async Task<List<PluginsResponeDTO>> GetAllActivePlugin()
        {
            var plugins = await _pluginRepository.GetAllAsync();
            return plugins.Select(plugin => new PluginsResponeDTO
            {
                Id = plugin.id,
                Name = plugin.Name,
                Category = plugin.Categoryid,
                Decription = plugin.Description
            }).ToList();
        }

        public async Task<object> Execute(int Id, object input)
        {
            try
            {
                
                var plugin = await _pluginmanagerRepository.GetByIdAsync(Id);

                var result = plugin.Execute(input);
                return result;
            }
            catch (PluginNotFound ex)
            {
                throw ex;
            }
            
        }

    }
}