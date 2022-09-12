using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Framework.Time;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Thread;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Components.InputBox;

public sealed partial class InputBoxViewModel :
    ViewModelBase,
    IAsyncEventHandler<LoadStartEvent>,
    IAsyncEventHandler<LoadEndEvent>,
    IAsyncNavigatedFrom
{
    private readonly IEventAggregator _eventAggregator;
    private InputBoxViewModelParameter? _parameter;
    private bool _loaded;

    private readonly ITypeProvider _typeProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMinDelay _minDelay;

    public InputBoxViewModel(
        IEventAggregator eventAggregator,
        ITypeProvider typeProvider,
        IDateTimeProvider dateTimeProvider,
        IMinDelay minDelay
    )
    {
        _eventAggregator = eventAggregator;
        _typeProvider = typeProvider;
        _dateTimeProvider = dateTimeProvider;
        _minDelay = minDelay;

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

    public Func<string, Task> ApplyAutoCompletionItemAsync => ApplyAutoCompletionAsync;

    private Task ApplyAutoCompletionAsync(string item)
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

    public Func<string, CancellationToken, Task<IEnumerable<string>>> LoadAutoCompletionItemsAsync => LoadAutoCompletionAsync;

    private async Task<IEnumerable<string>> LoadAutoCompletionAsync(string value, CancellationToken ct)
    {
        var start = _dateTimeProvider.Now();

        var types = _typeProvider.TryGetByName(value, new TypeEvaluationOptions { Regex = true, IgnoreCase = true }, ct);

        var ordered = types
            .Select(type => type.Name)
            .OrderBy(name => name.Length)
            .ThenBy(name => name)
            .Distinct();

        await _minDelay.DelayAsync(start, TimeSpan.FromSeconds(0.25)).ConfigureAwait(true);

        return ordered;
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

    public Task HandleAsync(LoadStartEvent e, CancellationToken? ct = null)
    {
        _loaded = false;
        ActionCommand.NotifyCanExecuteChanged();
        return Task.CompletedTask;
    }

    public Task HandleAsync(LoadEndEvent e, CancellationToken? ct = null)
    {
        _loaded = true;
        ActionCommand.NotifyCanExecuteChanged();
        return Task.CompletedTask;
    }
}