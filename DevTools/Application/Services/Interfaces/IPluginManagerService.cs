using DevTools.Dto.Category;
using DevTools.Dto.Plugins;
using DevTools.Dto.Querry;
using DevTool.UISchema;
using DevTools.Application.Dto.Plugins;
using DevTools.Application.Dto.File;

namespace DevTools.Services.Interfaces
{
    public interface IPluginManagerService
    {
        Task<PluginsResponeDTO> GetPLuginById(int id);
        Task<List<PluginsResponeDTO>> GetAllActivePlugin();
        Task<List<PluginResponeWithActiveStatusDTO>> GetAllPlugin();
        Task<List<PluginsResponeDTO>> GetAllByQuerry(PluginQuerry querry);

        Task UploadLoadPlugin(UploadFileDTO upload);
        Task SetPremiumStatus(int pluginId, bool status);
        Task SetActiveStatus(int pluginId, bool status);

        Task<object> Execute(int Id, object input);

        Task<PluginUI> GetScheme1(int id);

        Task<Schema> GetScheme(int id);
        Task DeletePluginByIdAsync(int pluginId);
    }
}