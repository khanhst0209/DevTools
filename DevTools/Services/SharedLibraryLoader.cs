using DevTools.Services.Interfaces;
using Services.AssemblyManager;

public class SharedLibraryLoader : ISharedLibraryLoader
{
    private readonly IAssemblyManager _assemblymanager;
    private readonly string _pluginFolder = "./Plugins/SharedLibrary";

    public SharedLibraryLoader(IAssemblyManager _assemblymanager)
    {
        this._assemblymanager = _assemblymanager;
    }

    public async Task LoadLibraryAsync()
    {
        Console.WriteLine("=================================================");
        Console.WriteLine("Load Library assembly");
        foreach (var file in Directory.GetFiles(_pluginFolder, "*.dll"))
        {
            await AddLibraryAsync(file);
        }
        Console.WriteLine("=================================================");
    }

    public async Task AddLibraryAsync(string path)
    {
        Console.WriteLine("++++++=========================++++++++");
        Console.WriteLine("Add Library into memory");
        await _assemblymanager.LoadAssemblyAsync(path);
        Console.WriteLine("++++++=========================++++++++");
    }

    public async Task RemoveLibraryAsync(string path)
    {
        Console.WriteLine("++++++=========================++++++++");
        Console.WriteLine("Remove Library out of memory");
        await _assemblymanager.UnloadAssemblyAsync(path);
        Console.WriteLine("++++++=========================++++++++");
    }
}
