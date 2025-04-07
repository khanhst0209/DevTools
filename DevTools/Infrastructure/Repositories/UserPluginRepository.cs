using AutoMapper;
using DevTools.data;
using DevTools.Dto.Plugins;
using DevTools.Dto.UserPlugin;
using DevTools.Exceptions.AccountManager.UserException;
using DevTools.Exceptions.Plugins.PluginsException.cs;
using DevTools.Exceptions.UserPlugin;
using DevTools.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.data;

namespace DevTools.Repositories
{
    public class UserPluginRepository : IUserPluginRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _user;
        private readonly IPluginRepository _pluginrepoitory;

        public UserPluginRepository(MyDbContext _context,
                                        IMapper _mapper,
                                        UserManager<User> _user,
                                        IPluginRepository _pluginrepoitory)
        {
            this._context = _context;
            this._mapper = _mapper;
            this._user = _user;
            this._pluginrepoitory = _pluginrepoitory;
        }

        private async Task IsValidInput(CreateUserPluginDTO userplugindto)
        {
            var user = await _user.FindByIdAsync(userplugindto.UserId);
            if (user == null)
            {
                throw new UserNotFound(userplugindto.UserId);
            }

            var plugin = await _pluginrepoitory.GetByIdAsync(userplugindto.PluginId);
            if (plugin == null)
            {
                throw new PluginNotFound(userplugindto.PluginId);
            }
        }
        public async Task AddFavoritePlugin(CreateUserPluginDTO userplugindto)
        {
            await IsValidInput(userplugindto);


            var temp = await _context.UserPlugins.
            FirstOrDefaultAsync(x => x.PluginId == userplugindto.PluginId
                                                    && x.UserId == userplugindto.UserId);

            if (temp != null)
            {
                throw new UserPLuginExistedException(userplugindto.UserId, userplugindto.PluginId);
            }

            var item = _mapper.Map<UserPlugins>(userplugindto);
            await _context.UserPlugins.AddAsync(item);
            await _context.SaveChangesAsync();

        }

        public async Task<List<UserPlugins>> GetPLuginsByUserId(string UserId)
        {
            return await _context.UserPlugins.Where(x => x.UserId == UserId).Include(x => x.plugin).ThenInclude(x => x.category).AsNoTracking().ToListAsync();
        }

        public async Task RemoveFavoritePlugin(CreateUserPluginDTO userplugindto)
        {
            await IsValidInput(userplugindto);

            var temp = await _context.UserPlugins.
            FirstOrDefaultAsync(x => x.PluginId == userplugindto.PluginId
                                                    && x.UserId == userplugindto.UserId);

            if (temp == null)
            {
                throw new UserPLuginExistedException(userplugindto.UserId, userplugindto.PluginId);
            }

            _context.UserPlugins.Remove(temp);
            await _context.SaveChangesAsync();
        }

    }
}