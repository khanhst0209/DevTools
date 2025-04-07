using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DevTools.Services.Interfaces;

public class PluginWatcherService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private FileSystemWatcher _watcher;

    public PluginWatcherService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _watcher = new FileSystemWatcher("Plugins/DevTool_Plugins", "*.dll")
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
        };

        _watcher.Created += async (s, e) => await OnPluginCreated(e.FullPath);
        _watcher.Deleted += async (s, e) => await OnPluginDeleted(e.FullPath);
        _watcher.Changed += async (s, e) => await OnPluginChanged(e.FullPath); // Thêm sự kiện Changed
        _watcher.EnableRaisingEvents = true;

        return Task.CompletedTask;
    }

    private async Task OnPluginCreated(string path)
    {
        using var scope = _serviceProvider.CreateScope();
        var pluginService = scope.ServiceProvider.GetRequiredService<IPluginLoader>();
        await pluginService.AddPluginAsync(path);
    }

    private async Task OnPluginDeleted(string path)
    {
        using var scope = _serviceProvider.CreateScope();
        var pluginService = scope.ServiceProvider.GetRequiredService<IPluginLoader>();
        await pluginService.RemovePluginAsync(path);
    }

    private async Task OnPluginChanged(string path)
    {
        using var scope = _serviceProvider.CreateScope();
        var pluginService = scope.ServiceProvider.GetRequiredService<IPluginLoader>();
        await pluginService.ReplacePluginAsync(path);  
    }

    public override void Dispose()
    {
        _watcher?.Dispose();
        base.Dispose();
    }
}
