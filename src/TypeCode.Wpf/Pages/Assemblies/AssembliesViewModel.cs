using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Business.Configuration;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Pages.Assemblies;

public partial class AssemblyViewModel : ObservableObject, IAsyncNavigatedTo, IAsyncNavigatedFrom, IAsyncEventHandler<LoadEndEvent>
{
    private readonly IModalNavigationService _modalNavigationService;
    private readonly ITypeProvider _typeProvider;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IEventAggregator _eventAggregator;

    public AssemblyViewModel(
        IModalNavigationService modalNavigationService,
        ITypeProvider typeProvider,
        IConfigurationProvider configurationProvider,
        IInputBoxViewModelFactory inputBoxViewModelFactory,
        IEventAggregator eventAggregator
    )
    {
        _modalNavigationService = modalNavigationService;
        _typeProvider = typeProvider;
        _configurationProvider = configurationProvider;
        _eventAggregator = eventAggregator;

        LoadedAssemblies = new List<string>();

        eventAggregator.Subscribe<LoadEndEvent>(this);

        var parameter = new InputBoxViewModelParameter("Search", SearchAsync)
        {
            ToolTip = "Input type name to search for."
        };

        InputBoxViewModel = inputBoxViewModelFactory.Create(parameter);
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        if (_configurationProvider.IsSet())
        {
            LoadAssemblies();
        }

        return Task.CompletedTask;
    }

    public Task OnNavigatedFromAsync(NavigationContext context)
    {
        _eventAggregator.Unsubscribe(this);
        return Task.CompletedTask;
    }
    
    [ObservableProperty]
    private InputBoxViewModel? _inputBoxViewModel;

    [ObservableProperty]
    private List<string>? _loadedAssemblies;

    public Task HandleAsync(LoadEndEvent e)
    {
        LoadAssemblies();
        return Task.CompletedTask;
    }

    private void LoadAssemblies()
    {
        var configuration = _configurationProvider.Get();
        LoadedAssemblies = configuration.AssemblyRoot
            .OrderBy(r => r.Priority)
            .SelectMany(r => r.AssemblyGroup)
            .SelectMany(r => r.PriorityAssemblyList)
            .OrderBy(r => r.Priority)
            .Select(r => $"{r.Priority} {r.Message}")
            .ToList();

        if (!LoadedAssemblies.Any())
        {
            LoadedAssemblies.Add("No assemblies have been loaded");
        }
    }

    private async Task SearchAsync(bool regex, string? input)
    {
        var types = _typeProvider.TryGetByName(input?.Trim(), new TypeEvaluationOptions { Regex = regex }).ToList();

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
}