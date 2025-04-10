using DevTools.data;
using DevTools.Dto.Plugins;
using DevTools.Dto.UserPlugin;

namespace DevTools.Repositories.Interfaces
{
    public interface IUserPluginRepository
    {
        Task AddFavoritePlugin(string userId, int pluginId);

        Task<List<UserPlugins>> GetPLuginsByUserId(string UserId);

        Task RemoveFavoritePlugin(string userId, int pluginId);
    }
}