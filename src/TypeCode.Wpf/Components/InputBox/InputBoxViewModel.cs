using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Components.InputBox;

public partial class InputBoxViewModel :
    ViewModelBase,
    IAsyncEventHandler<LoadStartEvent>,
    IAsyncEventHandler<LoadEndEvent>,
    IAsyncNavigatedFrom
{
    private readonly IEventAggregator _eventAggregator;
    private InputBoxViewModelParameter? _parameter;
    private bool _loaded;
    
    private readonly ITypeProvider _typeProvider;

    public InputBoxViewModel(
        IEventAggregator eventAggregator,
        ITypeProvider typeProvider
    )
    {
        _eventAggregator = eventAggregator;
        _typeProvider = typeProvider;

        _loaded = true;

        eventAggregator.Subscribe<LoadStartEvent>(this);
        eventAggregator.Subscribe<LoadEndEvent>(this);
    }

    public void Initialize(InputBoxViewModelParameter parameter)
    {
        ActionName = parameter.ActionName;
        ToolTip = parameter.ToolTip;
        _parameter = parameter;
    }

    public Task OnNavigatedFromAsync(NavigationContext context)
    {
        _eventAggregator.Unsubscribe(this);
        return Task.CompletedTask;
    }
    
    public Func<string, Task> ApplyAutoCompletionItemAsync => ApplySuggestionAsync;

    private Task ApplySuggestionAsync(string item)
    {
        Input = item;
        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanAction))]
    private Task ActionAsync()
    {
        return _parameter!.ActionAsync(UseRegexSearch, Input);
    }

    private bool CanAction()
    {
        return _loaded && !HasErrors && !string.IsNullOrEmpty(Input);
    }

    public Func<string, Task<IEnumerable<string>>> LoadAutoCompletionItemsAsync => LoadAutoCompletionAsync;

    private Task<IEnumerable<string>> LoadAutoCompletionAsync(string value)
    {
        var types = UseRegexSearch
            ? _typeProvider.TryGetByName(value, new TypeEvaluationOptions { Regex = true })
            : _typeProvider.TryGetTypesByCondition(type => type.Name.ToLowerInvariant().Contains(value.ToLowerInvariant()));

        var ordered = types
            .Select(type => type.Name)
            .OrderBy(name => name.Length)
            .ThenBy(name => name)
            .Distinct();

        return Task.FromResult(ordered);
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ActionCommand))]
    [NotifyDataErrorInfo]
    [Required]
    private string? _input;

    [ObservableProperty]
    private string? _toolTip;

    [ObservableProperty]
    private string? _actionName;

    [ObservableProperty]
    private bool _useRegexSearch;

    [ObservableProperty]
    private ObservableCollection<string> _suggestionItems = new();

    [ObservableProperty]
    private ObservableCollection<MenuItem> _contextMenu = new();

    public Task HandleAsync(LoadStartEvent e, CancellationToken? cancellationToken = null)
    {
        _loaded = false;
        ActionCommand.NotifyCanExecuteChanged();
        return Task.CompletedTask;
    }

    public Task HandleAsync(LoadEndEvent e, CancellationToken? cancellationToken = null)
    {
        _loaded = true;
        ActionCommand.NotifyCanExecuteChanged();
        return Task.CompletedTask;
    }
}