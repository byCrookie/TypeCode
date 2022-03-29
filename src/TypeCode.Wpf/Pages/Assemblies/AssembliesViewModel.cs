using System.Windows.Input;
using TypeCode.Business.Configuration;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Assemblies;

public class AssemblyViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly IModalNavigationService _modalNavigationService;
    private readonly ITypeProvider _typeProvider;
    private readonly IConfigurationProvider _configurationProvider;

    public AssemblyViewModel(
        IModalNavigationService modalNavigationService,
        ITypeProvider typeProvider,
        IConfigurationProvider configurationProvider
    )
    {
        _modalNavigationService = modalNavigationService;
        _typeProvider = typeProvider;
        _configurationProvider = configurationProvider;

        LoadedAssemblies = new List<string>();

        SearchCommand = new AsyncRelayCommand(SearchAsync);
    }

    public Task OnNavigatedToAsync(NavigationContext context)
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

    private async Task SearchAsync()
    {
        var types = _typeProvider.TryGetByName(Input, new TypeEvaluationOptions { Regex = Regex }).ToList();

        if (types.Any())
        {
            var typeNames = types.Select(type => $"{type.FullName}" +
                                                 $"{Environment.NewLine}-{type.AssemblyQualifiedName}" +
                                                 $"{Environment.NewLine}-{type.Assembly.Location}");
            await _modalNavigationService.OpenModalAsync(new ModalParameter
            {
                Title = $"Valid Type-Name {Input}",
                Text = $"Types: {Environment.NewLine}{string.Join($"{Environment.NewLine}{Environment.NewLine}", typeNames)}"
            }).ConfigureAwait(true);
        }
        else
        {
            await _modalNavigationService.OpenModalAsync(new ModalParameter
            {
                Title = $"Invalid Type-Name {Input}",
                Text = $"No types were found for name {Input}. {Environment.NewLine}" +
                       $"{Environment.NewLine}Possible Reasons:" +
                       $"{Environment.NewLine}- Wrong typename" +
                       $"{Environment.NewLine}- Type does not exist in configured assemblies" +
                       $"{Environment.NewLine}- Type was not loaded because of corrupt assembly" +
                       $"{Environment.NewLine}- Type was not loaded because of error while executing TypeCode"
            }).ConfigureAwait(true);
        }
    }

    public ICommand SearchCommand { get; set; }

    public string? Input
    {
        get => Get<string?>();
        set => Set(value);
    }

    public bool Regex
    {
        get => Get<bool>();
        set => Set(value);
    }

    public List<string>? LoadedAssemblies
    {
        get => Get<List<string>?>();
        private set => Set(value);
    }
}