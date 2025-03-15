using System.IO;
namespace Plugins.Manager;
using System;
using System.IO;

public static class PluginWatcher
{
    private static FileSystemWatcher _watcher;

    public static void StartWatching()
    {
        _watcher = new FileSystemWatcher("Plugins/DevTool_Plugins", "*.dll")
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
        };

        _watcher.Created += (s, e) => ReloadPlugins();
        _watcher.Changed += (s, e) => ReloadPlugins();
        _watcher.Deleted += (s, e) => ReloadPlugins();

        _watcher.EnableRaisingEvents = true;
    }

    private static void ReloadPlugins()
    {
        Console.WriteLine("🔄 Plugin folder changed. Reloading plugins...");
        PluginManager.LoadPlugins();
    }
}
