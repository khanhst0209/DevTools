


using DevTools.Services.Interfaces;

public class LibraryWatcherService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private FileSystemWatcher _watcher;

    public LibraryWatcherService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _watcher = new FileSystemWatcher("Plugins/SharedLibrary", "*.dll")
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
        };

        _watcher.Created += async (s, e) => await OnLibraryCreated(e.FullPath);
        _watcher.Deleted += async (s, e) => await OnLibraryDeleted(e.FullPath);
        _watcher.EnableRaisingEvents = true;

        return Task.CompletedTask;
    }

    private async Task OnLibraryCreated(string path)
    {
        using var scope = _serviceProvider.CreateScope();
        var libService = scope.ServiceProvider.GetRequiredService<ISharedLibraryLoader>();
        await libService.AddLibraryAsync(path);
    }

    private async Task OnLibraryDeleted(string path)
    {
        using var scope = _serviceProvider.CreateScope();
        var libService = scope.ServiceProvider.GetRequiredService<ISharedLibraryLoader>();
        await libService.RemoveLibraryAsync(path);
    }

    public override void Dispose()
    {
        _watcher?.Dispose();
        base.Dispose();
    }
}
