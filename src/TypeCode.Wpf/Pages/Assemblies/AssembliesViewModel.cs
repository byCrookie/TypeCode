using TypeCode.Business.Configuration;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Components.SearchBox;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Assemblies;

public class AssemblyViewModel : Reactive, IAsyncEventHandler<LoadEndEvent>
{
    private readonly IModalNavigationService _modalNavigationService;
    private readonly ITypeProvider _typeProvider;
    private readonly IConfigurationProvider _configurationProvider;

    public AssemblyViewModel(
        IModalNavigationService modalNavigationService,
        ITypeProvider typeProvider,
        IConfigurationProvider configurationProvider,
        ISearchBoxViewModelFactory searchBoxViewModelFactory,
        IEventAggregator eventAggregator
    )
    {
        _modalNavigationService = modalNavigationService;
        _typeProvider = typeProvider;
        _configurationProvider = configurationProvider;

        LoadedAssemblies = new List<string>();
        
        eventAggregator.Subscribe<LoadEndEvent>(this);

        SearchBoxViewModel = searchBoxViewModelFactory.Create(SearchAsync);
    }

    private async Task SearchAsync(bool regex, string? input)
    {
        var types = _typeProvider.TryGetByName(input, new TypeEvaluationOptions { Regex = regex }).ToList();

        if (types.Any())
        {
            var typeNames = types.Select(type => $"{type.FullName}" +
                                                 $"{Environment.NewLine}-{type.AssemblyQualifiedName}" +
                                                 $"{Environment.NewLine}-{type.Assembly.Location}");
            await _modalNavigationService.OpenModalAsync(new ModalParameter
            {
                Title = $"Valid Type-Name {input}",
                Text = $"Types: {Environment.NewLine}{string.Join($"{Environment.NewLine}{Environment.NewLine}", typeNames)}"
            }).ConfigureAwait(true);
        }
        else
        {
            await _modalNavigationService.OpenModalAsync(new ModalParameter
            {
                Title = $"Invalid Type-Name {input}",
                Text = $"No types were found for name {input}. {Environment.NewLine}" +
                       $"{Environment.NewLine}Possible Reasons:" +
                       $"{Environment.NewLine}- Wrong typename" +
                       $"{Environment.NewLine}- Type does not exist in configured assemblies" +
                       $"{Environment.NewLine}- Type was not loaded because of corrupt assembly" +
                       $"{Environment.NewLine}- Type was not loaded because of error while executing TypeCode"
            }).ConfigureAwait(true);
        }
    }

    public SearchBoxViewModel? SearchBoxViewModel
    {
        get => Get<SearchBoxViewModel?>();
        set => Set(value);
    }

    public List<string>? LoadedAssemblies
    {
        get => Get<List<string>?>();
        private set => Set(value);
    }

    public Task HandleAsync(LoadEndEvent e)
    {
        var configuration = _configurationProvider.GetConfiguration();
        LoadedAssemblies = configuration.AssemblyRoot
            .OrderBy(r => r.Priority)
            .SelectMany(r => r.AssemblyGroup)
            .SelectMany(r => r.PriorityAssemblyList)
            .OrderBy(r => r.Priority)
            .Select(r => $"{r.Priority} {r.Message}")
            .ToList();
        return Task.CompletedTask;
    }
}