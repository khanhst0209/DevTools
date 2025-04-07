namespace DevTools.Exceptions.Plugins.PluginsException.cs
{
    public class PluginNotFound : Exception
    {
        public PluginNotFound(int Id) : base($"Plugin with Id : {Id} is not Found"){}
    }
}