namespace DevTools.Services.Interfaces
{
    public interface ISharedLibraryLoader
    {
         Task LoadLibraryAsync();
         Task AddLibraryAsync(string path);
         Task RemoveLibraryAsync(string path);
    }
}