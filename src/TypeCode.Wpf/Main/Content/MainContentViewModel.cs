using AsyncAwaitBestPractices;
using TypeCode.Business.Version;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Main.Content;

public class MainContentViewModel : Reactive, IAsyncEventHandler<AssemblyLoadedEvent>, IAsyncEventHandler<VersionEvent>
{
    public MainContentViewModel(IEventAggregator eventAggregator, IVersionEvaluator versionEvaluator)
    {
        AreAssembliesLoading = true;

        async Task CheckVersion()
        {
            var version = await versionEvaluator.EvaluateAsync().ConfigureAwait(false);
            await eventAggregator.PublishAsync(new VersionEvent { Version = version }).ConfigureAwait(false);
        }

        eventAggregator.Subscribe<VersionEvent>(this);
        eventAggregator.Subscribe<AssemblyLoadedEvent>(this);
        
        CheckVersion().SafeFireAndForget();
    }
        
    public bool AreAssembliesLoading {
        get => Get<bool>();
        set => Set(value);
    }
    
    public bool HasNewVersion {
        get => Get<bool>();
        set => Set(value);
    }
    
    public string? VersionMessage {
        get => Get<string?>();
        set => Set(value);
    }
    
    public string? VersionLink {
        get => Get<string?>();
        set => Set(value);
    }

    public Task HandleAsync(AssemblyLoadedEvent e)
    {
        AreAssembliesLoading = false;
        return Task.CompletedTask;
    }

    public Task HandleAsync(VersionEvent e)
    {
        if (e.Version is not null)
        {
            HasNewVersion = true;
            VersionMessage = $"New version {e.Version} available at https://github.com/byCrookie/TypeCode/releases/tag/{e.Version}";
            VersionLink = $"https://github.com/byCrookie/TypeCode/releases/tag/{e.Version}";
        }
        
        return Task.CompletedTask;
    }
}