namespace DevTools.Services.Interfaces
{
    public interface IPluginLoader
    {
        Task LoadPluginsAsync();
        Task AddPluginAsync(string path);
        Task RemovePluginAsync(string path);

        Task ReplacePluginAsync(string path);
        Task<bool> LoadPluginFromFile(string dllPath);
    }
}