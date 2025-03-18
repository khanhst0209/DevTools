using DevTools.Exceptions.Plugins.PluginsException.cs;
using DevTools.Repositories.Interfaces;
using Plugins.DevTool;

namespace DevTools.Repositories
{
    public class PluginManagerRepository : IPluginManagerRepository
    {
        private static readonly List<IDevToolPlugin> _plugins = new();

        public PluginManagerRepository() { }

        public async Task AddAsync(IDevToolPlugin plugin)
        {
            var item = _plugins.FirstOrDefault(x => x.id == plugin.id);
            

            if (item != null)
                return;

            _plugins.Add(plugin);
            Console.WriteLine($"    ---Added plugin: {plugin.Name} into tools ");
        }

        public async Task<bool> CheckExisted(int Id)
        {
            return _plugins.FirstOrDefault(x => x.id == Id) != null;
        }


        public async Task ClearAsync()
        {
            _plugins.Clear();
        }

        public async Task<List<IDevToolPlugin>> GetAll()
        {
            return _plugins;
        }

        public async Task<IDevToolPlugin> GetByIdAsync(int Id)
        {
            // foreach(var k in _plugins)
            // {
            //     Console.WriteLine($"Id ne : {k.id} hehe");
            // }
            var item = _plugins.FirstOrDefault(x => x.id == Id);
            if (item != null)
                return item;

            throw new PluginNotFound(Id);
        }

        public async Task RemoveAsync(int Id)
        {
            var item = _plugins.FirstOrDefault(x => x.id == Id);

            if (item == null)
                return;

            _plugins.Remove(item);
        }

    }
}