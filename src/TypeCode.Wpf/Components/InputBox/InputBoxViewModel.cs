using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyInjection.Factory;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply.Command;
using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply.Event;
using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Item;
using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Open;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Thread;

namespace TypeCode.Wpf.Components.InputBox;

public partial class InputBoxViewModel :
    ObservableValidator,
    IAsyncEventHandler<LoadStartEvent>,
    IAsyncEventHandler<LoadEndEvent>,
    IAsyncNavigatedTo,
    IAsyncNavigatedFrom
{
    private readonly IEventAggregator _eventAggregator;
    private InputBoxViewModelParameter? _parameter;
    private bool _loaded;

    private readonly IApplySuggestionCommandExecutor _applySuggestionCommandExecutor;
    private readonly ITypeProvider _typeProvider;

    private readonly IApplySuggestionEventHandler _applySuggestionEventHandler;
    private readonly IBuildSuggestionEventHandler _buildSuggestionEventHandler;

    public InputBoxViewModel(
        IEventAggregator eventAggregator,
        IFactory<IApplySuggestionEventHandler> applySuggestionEventHandlerFactory,
        IFactory<IBuildSuggestionEventHandler> openSuggestionEventHandlerFactory,
        IApplySuggestionCommandExecutor applySuggestionCommandExecutor,
        ITypeProvider typeProvider
    )
    {
        _eventAggregator = eventAggregator;
        _applySuggestionCommandExecutor = applySuggestionCommandExecutor;
        _typeProvider = typeProvider;

        _loaded = true;


        eventAggregator.Subscribe<LoadStartEvent>(this);
        eventAggregator.Subscribe<LoadEndEvent>(this);

        _applySuggestionEventHandler = applySuggestionEventHandlerFactory.Create();
        _buildSuggestionEventHandler = openSuggestionEventHandlerFactory.Create();
    }

    public void Initialize(InputBoxViewModelParameter parameter)
    {
        ActionName = parameter.ActionName;
        ToolTip = parameter.ToolTip;
        _parameter = parameter;
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        _applySuggestionEventHandler.Initialize(new ApplySuggestionEventHandlerParameter(this));
        _buildSuggestionEventHandler.Initialize(new BuildSuggestionEventHandlerParameter(this));
        return Task.CompletedTask;
    }

    public Task OnNavigatedFromAsync(NavigationContext context)
    {
        _applySuggestionEventHandler.Dispose();
        _buildSuggestionEventHandler.Dispose();

        _eventAggregator.Unsubscribe(this);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task ApplySuggestionAsync()
    {
        await _applySuggestionCommandExecutor.ExecuteAsync(CreateApplySuggestionCommandParameter()).ConfigureAwait(true);
        SuggestionItems.Clear();
        IsFunctionDropDownOpen = false;
    }

    private ApplySuggestionCommandParameter CreateApplySuggestionCommandParameter()
    {
        return new ApplySuggestionCommandParameter(new SuggestionItem(SelectedSuggestionItem?.Suggestion), this);
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

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ActionCommand))]
    [NotifyDataErrorInfo]
    [Required]
    private string? _input;

    partial void OnInputChanged(string? value)
    {
        MainThread.BackgroundFireAndForget(() =>
        {
            if (!string.IsNullOrEmpty(value))
            {
                var types = UseRegexSearch
                    ? _typeProvider.TryGetByName(value, new TypeEvaluationOptions { Regex = true })
                    : _typeProvider.TryGetTypesByCondition(type => type.Name.Contains(value));

                _eventAggregator.PublishAsync(new BuildSuggestionEvent(types.OrderBy(type => type.Name).DistinctBy(type => type.Name), this));
            }

            IsFunctionDropDownOpen = !string.IsNullOrEmpty(value) && SuggestionItems.Any();
        }, DispatcherPriority.Background);
    }

    [ObservableProperty]
    private string? _toolTip;

    [ObservableProperty]
    private string? _actionName;

    [ObservableProperty]
    private bool _useRegexSearch;

    [ObservableProperty]
    private bool _isFunctionDropDownOpen;

    [ObservableProperty]
    private ObservableCollection<SuggestionItemViewModel> _suggestionItems = new();

    [ObservableProperty]
    private SuggestionItemViewModel? _selectedSuggestionItem;

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