using DevTools.data;
using DevTools.Dto.Category;
using DevTools.Dto.Plugins;
using DevTools.Repositories;
using DevTools.Repositories.Interfaces;
using DevTools.Services.Interfaces;

namespace DevTools.Services
{
    public class PluginCategoryService : IPluginCategoryService
    {
        private readonly IPluginCategoryRepository _pluginCategoryRepository;

        public PluginCategoryService(IPluginCategoryRepository _pluginCategoryRepository)
        {
            this._pluginCategoryRepository = _pluginCategoryRepository;
        }

        public async Task<List<PLuginCategoryDTO>> GetAllCategoryWithplugin()
        {
            var categories = await _pluginCategoryRepository.GetAllAsync();
            var result = categories.Select(x => new PLuginCategoryDTO
            {
                Id = x.Id,
                Name = x.Name,
                plugins = x.Plugins?.Select(y => new PluginMinimize
                {
                    Id = y.Id,
                    Name = y.Name
                }).ToList() ?? new List<PluginMinimize>()
            }).ToList();

            return result;
        }


    }
}