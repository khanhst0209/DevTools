using Plugins.DevTool;

namespace DevTools.Repositories.Interfaces
{
    public interface IPluginManagerRepository
    {
        Task<IDevToolPlugin> GetByIdAsync(int Id);
        Task AddAsync(IDevToolPlugin plugin);
        Task RemoveAsync(int Id);
        Task<List<IDevToolPlugin>> GetAll();
        Task ClearAsync();

        Task<bool> CheckExisted(int Id);


        Task<string> GetScheme1(int id);
        Task<Object> GetScheme2(int id);
    }
}