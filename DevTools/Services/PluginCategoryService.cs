using AutoMapper;
using DevTools.data;
using DevTools.Dto.Category;
using DevTools.Repositories.Interfaces;
using DevTools.Services.Interfaces;

public class PluginCategoryService : IPluginCategoryService
{
    private readonly IPluginCategoryRepository _pluginCategoryRepository;
    private readonly IMapper _mapper;

    public PluginCategoryService(IPluginCategoryRepository pluginCategoryRepository, IMapper mapper)
    {
        _pluginCategoryRepository = pluginCategoryRepository;
        _mapper = mapper;
    }

    public async Task<List<PLuginCategoryDTO>> GetAllCategoryWithplugin()
    {
        var categories = await _pluginCategoryRepository.GetAllAsync();
        return _mapper.Map<List<PLuginCategoryDTO>>(categories);
    }
}
