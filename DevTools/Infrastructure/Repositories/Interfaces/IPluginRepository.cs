using DevTools.data;
using DevTools.Dto.Plugins;
using DevTools.Dto.Querry;

namespace DevTools.Repositories.Interfaces
{
    public interface IPluginRepository : IBaseRepository<Plugin, int>
    {
         Task<List<Plugin>> GetAllByQuerryAsync(PluginQuerry querry);
         Task AddPluginAsync(CreatePluginDTO createplugindto);

         Task<Plugin> GetByName(string name);

         Task CheckPluginExistInFoulder();

    }
}