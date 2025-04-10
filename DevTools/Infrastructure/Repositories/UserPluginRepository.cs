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

        private async Task IsValidInput(string userId, int pluginId)
        {

            var user = await _user.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFound(userId);
            }

            var plugin = await _pluginrepoitory.GetByIdAsync(pluginId);
            if (plugin == null)
            {
                throw new PluginNotFound(pluginId);
            }
        }
        public async Task AddFavoritePlugin(string userId, int pluginId)
        {
            await IsValidInput(userId, pluginId);


            var temp = await _context.UserPlugins.
            FirstOrDefaultAsync(x => x.PluginId == pluginId
                                                    && x.UserId == userId);

            if (temp != null)
            {
                throw new UserPLuginExistedException(userId, pluginId);
            }

            var item = new UserPlugins
            {
                UserId = userId,
                PluginId = pluginId
            };
            await _context.UserPlugins.AddAsync(item);
            await _context.SaveChangesAsync();

        }

        public async Task<List<UserPlugins>> GetPLuginsByUserId(string UserId)
        {
            return await _context.UserPlugins.Where(x => x.UserId == UserId).Include(x => x.plugin).ThenInclude(x => x.category).AsNoTracking().ToListAsync();
        }

        public async Task RemoveFavoritePlugin(string userId, int pluginId)
        {
            await IsValidInput(userId, pluginId);

            var temp = await _context.UserPlugins.
            FirstOrDefaultAsync(x => x.PluginId == pluginId
                                                    && x.UserId == userId);

            if (temp == null)
            {
                throw new UserPLuginExistedException(userId, pluginId);
            }

            _context.UserPlugins.Remove(temp);
            await _context.SaveChangesAsync();
        }

    }
}