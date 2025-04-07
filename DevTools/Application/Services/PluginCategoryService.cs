using AutoMapper;
using DevTools.data;
using DevTools.Dto.Category;
using DevTools.Repositories.Interfaces;
using DevTools.Services.Interfaces;
using MyWebAPI.Exceptions;

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

    public async Task<PLuginCategoryDTO> GetCategoryById(int id)
    {
        var Category = await _pluginCategoryRepository.GetByIdAsync(id);

        if (Category == null)
            throw new CategoryNotFoundException(id);

        return _mapper.Map<PLuginCategoryDTO>(Category);
    }

}
