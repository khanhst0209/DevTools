
namespace DevTools.Exceptions.Plugins.PluginCategoryException.cs
{
    public class PluginCategoryNotFound : Exception
    {
        public PluginCategoryNotFound(int id) : base($"Plugin Category id : {id} is Not Found") { }
        public PluginCategoryNotFound(string name) : base($"Plugin Category name : {name} is Not Found") { }
    }
}