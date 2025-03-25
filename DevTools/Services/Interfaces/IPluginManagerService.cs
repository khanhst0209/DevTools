using DevTools.Dto.Plugins;
using Plugins.DevTool;

namespace DevTools.Services.Interfaces
{
    public interface IPluginManagerService
    {
        Task LoadPlugins();
        Task AddPlugin(string path);
        Task RemovePlugin(string path);
        Task<List<PluginsResponeDTO>> GetAllActivePlugin();

        Task<object> Execute(int Id, object input);

        Task<List<PluginsResponeDTO>> FindPluginByName(string name);
    }
}