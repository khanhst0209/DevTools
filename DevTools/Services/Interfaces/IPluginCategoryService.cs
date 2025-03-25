using DevTools.data;
using DevTools.Dto.Category;

namespace DevTools.Services.Interfaces
{
    public interface IPluginCategoryService
    {
         Task<List<PLuginCategoryDTO>> GetAllCategoryWithplugin();
    }
}