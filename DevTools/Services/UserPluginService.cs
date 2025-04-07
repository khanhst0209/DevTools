using AutoMapper;
using DevTools.Dto.Plugins;
using DevTools.Dto.UserPlugin;
using DevTools.Repositories.Interfaces;
using DevTools.Services.Interfaces;

namespace DevTools.Services
{
    public class UserPluginService : IUserPluginService
    {
        private readonly IUserPluginRepository _userPluginRepository;
        private readonly IMapper _mapper;
        public UserPluginService(IUserPluginRepository _userPluginRepository,
                                    IMapper _mapper)
        {
            this._userPluginRepository = _userPluginRepository;
            this._mapper = _mapper;
        }
        public async Task AddFavoritePlugin(CreateUserPluginDTO createUserPlugin)
        {
            await _userPluginRepository.AddFavoritePlugin(createUserPlugin);
        }

        public async Task<List<PluginsResponeDTO>> GetAllByUserId(string userId)
        {
            var items = await _userPluginRepository.GetPLuginsByUserId(userId);
            var plugins = _mapper.Map<List<PluginsResponeDTO>>(items.Select(x => x.plugin));
            return plugins;
        }

        public async Task RemoveFavoritePlugin(CreateUserPluginDTO createUserPlugin)
        {
            await _userPluginRepository.RemoveFavoritePlugin(createUserPlugin);
        }

    }
}