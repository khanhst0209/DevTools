using DevTools.Dto.Category;
using DevTools.Dto.Plugins;
using DevTools.Dto.Querry;
using DevTool.UISchema;

namespace DevTools.Services.Interfaces
{
    public interface IPluginManagerService
    {
        Task<PluginsResponeDTO> GetPLuginById(int id);
        Task<List<PluginsResponeDTO>> GetAllActivePlugin();
        Task<List<PluginsResponeDTO>> GetAllByQuerry(PluginQuerry querry);
        Task SetPremiumStatus(int pluginId, bool status);
        Task SetActiveStatus(int pluginId, bool status);

        Task<object> Execute(int Id, object input);

        Task<PluginUI> GetScheme1(int id);

        Task<Schema> GetScheme(int id);

    }
}