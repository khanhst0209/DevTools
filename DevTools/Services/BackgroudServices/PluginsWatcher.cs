using System.IO;
using System;
using System.IO;
using Plugins.Manager;
namespace Services.BackgroundServices.PluginsWatchers;


public static class PluginWatcher
{
    private static FileSystemWatcher _watcher;

    public static void StartWatching()
    {
        _watcher = new FileSystemWatcher("Plugins/DevTool_Plugins", "*.dll")
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
        };

        _watcher.Created += OnCreated;
        _watcher.Changed += (s, e) => ReloadPlugins();
        _watcher.Deleted += OnDeleted;

        _watcher.EnableRaisingEvents = true;
    }

    private static void ReloadPlugins()
    {
        Console.WriteLine("🔄 Plugin folder changed. Reloading plugins...");
        PluginManager.LoadPlugins();
    }

    private static void OnCreated(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine("🔄 Plugin folder Created. Adding New Plugin");
        PluginManager.AddPlugin(e.FullPath);
    }

    private static void OnDeleted(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"🗑️ Plugin bị xóa: {e.FullPath}");
        PluginManager.RemovePlugin(e.FullPath);
    }
}
