using DevTools.data;
using DevTools.Dto.Plugins;

namespace DevTools.Repositories.Interfaces
{
    public interface IPluginRepository
    {
         Task<List<Plugin>> GetAllAsync();
         Task<List<Plugin>> FindByNameAsync(string name);
         Task<Plugin> GetByIdAsync(int Id);
         Task<List<Plugin>> GetAllByCategoryAsync(int CategoryId);
         Task AddPluginAsync(CreatePluginDTO createplugindto);
         Task ActivateAsync(int pluginId, bool Isactivate);
         Task PremiumAsync(int pluginId, bool IsPremium);
         Task RemovePluginAsync(int pluginId);

         Task<int> GetIdByName(string name);

         Task CheckPluginExistInFoulder();
    }
}