using DevTools.data;
using DevTools.Dto.Plugins;
using DevTools.Dto.Querry;

namespace DevTools.Repositories.Interfaces
{
    public interface IPluginRepository
    {
         Task<List<Plugin>> GetAllAsync();
         Task<List<Plugin>> GetAllByQuerryAsync(PluginQuerry querry);
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