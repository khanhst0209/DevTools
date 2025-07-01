using DevTools.Application.Exceptions;

namespace DevTools.Exceptions.Plugins.PluginsException.cs
{
    public class PluginNotFound : NotFoundException
    {
        public PluginNotFound(int Id) : base($"Plugin with Id : {Id} is not Found") { }
    }
}