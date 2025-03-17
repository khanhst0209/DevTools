using DevTools.data;

namespace DevTools.Repositories.Interfaces
{
    public interface IPluginCategoryRepository
    {
         Task<List<PluginCategory>> GetAllAsync();
         Task<PluginCategory> GetByIdAsync(int Id);
         Task<bool> CheckExistByNameAsync(string name);
         Task<int> GetIdByName(string name);
         Task AddPluginCategoryAsync(string name);

         Task RemoveByIdAsync(int Id);
    }
}