using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.Configuration.Advanced;

namespace TypeCode.Wpf.Pages.Configuration.Simple;

public partial class SimpleConfigurationWizardViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly ISimpleConfigurator _simpleConfigurator;
    private readonly IAdvancedConfigurationViewModelFactory _advancedConfigurationViewModelFactory;

    public SimpleConfigurationWizardViewModel(ISimpleConfigurator simpleConfigurator, IAdvancedConfigurationViewModelFactory advancedConfigurationViewModelFactory)
    {
        _simpleConfigurator = simpleConfigurator;
        _advancedConfigurationViewModelFactory = advancedConfigurationViewModelFactory;

        AddRootCommand = new AsyncRelayCommand(() => _simpleConfigurator.AddRootAsync(SelectedItem!), () => _simpleConfigurator.CanAddRoot(SelectedItem));
        AddGroupCommand = new AsyncRelayCommand(() => _simpleConfigurator.AddGroupAsync(SelectedItem!), () => _simpleConfigurator.CanAddGroup(SelectedItem));
        AddIncludePatternCommand = new AsyncRelayCommand(() => _simpleConfigurator.AddIncludePatternAsync(SelectedItem!), () => _simpleConfigurator.CanAddIncludePattern(SelectedItem));
        AddPathCommand = new AsyncRelayCommand(() => _simpleConfigurator.AddPathAsync(SelectedItem!), () => _simpleConfigurator.CanAddPath(SelectedItem));
        AddSelectorCommand = new AsyncRelayCommand(() => _simpleConfigurator.AddSelectorAsync(SelectedItem!), () => _simpleConfigurator.CanAddSelector(SelectedItem));
        UpdateCommand = new AsyncRelayCommand(() => _simpleConfigurator.UpdateAsync(SelectedItem!), () => _simpleConfigurator.CanUpdate(SelectedItem));
        DeleteCommand = new AsyncRelayCommand(() => _simpleConfigurator.DeleteAsync(SelectedItem!), () => _simpleConfigurator.CanDelete(SelectedItem));
    }

    public async Task OnInititalNavigationAsync(NavigationContext context)
    {
        AdvancedConfigurationViewModel = await _advancedConfigurationViewModelFactory.CreateAsync().ConfigureAwait(true);
        var treeViewItem = await _simpleConfigurator.InitializeAsync().ConfigureAwait(true);
        TreeViewItems = new List<TreeViewItem> { treeViewItem };

        context.AddParameter(_simpleConfigurator);

        Refresh();
    }

    public AsyncRelayCommand AddRootCommand { get; }
    public AsyncRelayCommand AddGroupCommand { get; }
    public AsyncRelayCommand AddIncludePatternCommand { get; }
    public AsyncRelayCommand AddPathCommand { get; }
    public AsyncRelayCommand AddSelectorCommand { get; }
    public AsyncRelayCommand UpdateCommand { get; }
    public AsyncRelayCommand DeleteCommand { get; }

    [ObservableProperty]
    private List<TreeViewItem>? _treeViewItems;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddRootCommand))]
    [NotifyCanExecuteChangedFor(nameof(AddGroupCommand))]
    [NotifyCanExecuteChangedFor(nameof(AddIncludePatternCommand))]
    [NotifyCanExecuteChangedFor(nameof(AddPathCommand))]
    [NotifyCanExecuteChangedFor(nameof(AddSelectorCommand))]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private TreeViewItem? _selectedItem;

    [ObservableProperty]
    [ChildViewModel]
    private AdvancedConfigurationWizardViewModel? _advancedConfigurationViewModel;

    private void Refresh()
    {
        AddRootCommand.NotifyCanExecuteChanged();
        AddIncludePatternCommand.NotifyCanExecuteChanged();
        AddGroupCommand.NotifyCanExecuteChanged();
        AddPathCommand.NotifyCanExecuteChanged();
        AddSelectorCommand.NotifyCanExecuteChanged();
        UpdateCommand.NotifyCanExecuteChanged();
        DeleteCommand.NotifyCanExecuteChanged();
    }
}