using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.Common.Configuration;

public partial class SetupWizardViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly ISetupConfigurator _setupConfigurator;
    private readonly IConfigurationViewModelFactory _configurationViewModelFactory;

    public SetupWizardViewModel(ISetupConfigurator setupConfigurator, IConfigurationViewModelFactory configurationViewModelFactory)
    {
        _setupConfigurator = setupConfigurator;
        _configurationViewModelFactory = configurationViewModelFactory;

        AddRootCommand = new AsyncRelayCommand(() =>  _setupConfigurator.AddRootAsync(SelectedItem!), () => _setupConfigurator.CanAddRoot(SelectedItem));
        AddGroupCommand = new AsyncRelayCommand(() =>  _setupConfigurator.AddGroupAsync(SelectedItem!), () => _setupConfigurator.CanAddGroup(SelectedItem));
        AddIncludePatternCommand = new AsyncRelayCommand(() =>  _setupConfigurator.AddIncludePatternAsync(SelectedItem!), () => _setupConfigurator.CanAddIncludePattern(SelectedItem));
        AddPathCommand = new AsyncRelayCommand(() =>  _setupConfigurator.AddPathAsync(SelectedItem!), () => _setupConfigurator.CanAddPath(SelectedItem));
        AddSelectorCommand = new AsyncRelayCommand(() =>  _setupConfigurator.AddSelectorAsync(SelectedItem!), () => _setupConfigurator.CanAddSelector(SelectedItem));
        UpdateCommand = new AsyncRelayCommand(() =>  _setupConfigurator.UpdateAsync(SelectedItem!), () => _setupConfigurator.CanUpdate(SelectedItem));
        DeleteCommand = new AsyncRelayCommand(() =>  _setupConfigurator.DeleteAsync(SelectedItem!), () => _setupConfigurator.CanDelete(SelectedItem));
    }

    public async Task OnInititalNavigationAsync(NavigationContext context)
    {
        ConfigurationViewModel = await _configurationViewModelFactory.CreateAsync().ConfigureAwait(true);
        var treeViewItem = await _setupConfigurator.InitializeAsync().ConfigureAwait(true);
        TreeViewItems = new List<TreeViewItem> { treeViewItem };
        
        context.AddParameter(_setupConfigurator);
        
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
    private ConfigurationWizardViewModel? _configurationViewModel;
    
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