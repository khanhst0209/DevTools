using DevTool.Categories;
using DevTools.data;

namespace DevTools.Repositories.Interfaces
{
    public interface IPluginCategoryRepository : IBaseRepository<PluginCategory>
    {
         Task<PluginCategory> GetByName(string name);

    }
}