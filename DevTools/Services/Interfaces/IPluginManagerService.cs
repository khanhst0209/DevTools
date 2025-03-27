using DevTools.Dto.Category;
using DevTools.Dto.Plugins;
using DevTools.Dto.Querry;
using Plugins.DevTool;

namespace DevTools.Services.Interfaces
{
    public interface IPluginManagerService
    {
        Task LoadPlugins();
        Task AddPlugin(string path);
        Task RemovePlugin(string path);
        Task<List<PluginsResponeDTO>> GetAllActivePlugin();
        Task<List<PluginsResponeDTO>> GetAllByQuerry(PluginQuerry querry);

        Task<object> Execute(int Id, object input);

    }
}