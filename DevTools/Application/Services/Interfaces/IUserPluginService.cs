using DevTools.Dto.Plugins;
using DevTools.Dto.UserPlugin;

namespace DevTools.Services.Interfaces
{
    public interface IUserPluginService
    {
         Task<List<PluginsResponeDTO>> GetAllByUserId(string userId);
        Task RemoveFavoritePlugin(string userId, int pluginId);

        Task AddFavoritePlugin(string userId, int pluginId);
    }
}