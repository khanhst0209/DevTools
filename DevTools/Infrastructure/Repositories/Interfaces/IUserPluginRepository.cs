using DevTools.data;
using DevTools.Dto.Plugins;
using DevTools.Dto.UserPlugin;

namespace DevTools.Repositories.Interfaces
{
    public interface IUserPluginRepository
    {
        Task AddFavoritePlugin(CreateUserPluginDTO userplugindto);

        Task<List<UserPlugins>> GetPLuginsByUserId(string UserId);

        Task RemoveFavoritePlugin(CreateUserPluginDTO userplugindto);
    }
}