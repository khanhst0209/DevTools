using DevTool.Categories;
using DevTools.data;

namespace DevTools.Repositories.Interfaces
{
    public interface IPluginCategoryRepository : IBaseRepository<PluginCategory, int>
    {
         Task<PluginCategory> GetByName(string name);

    }
}