using System.Windows.Controls;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Common.Configuration;

public class SetupWizardViewModel : Reactive, IAsyncInitialNavigated
{
    private readonly ISetupConfigurator _setupConfigurator;
    private readonly IConfigurationViewModelFactory _configurationViewModelFactory;

    public SetupWizardViewModel(ISetupConfigurator setupConfigurator, IConfigurationViewModelFactory configurationViewModelFactory)
    {
        _setupConfigurator = setupConfigurator;
        _configurationViewModelFactory = configurationViewModelFactory;

        AddRootCommand = new AsyncRelayCommand(() =>  _setupConfigurator.AddRootAsync(SelectedItem!), _ => _setupConfigurator.CanAddRoot(SelectedItem));
        AddGroupCommand = new AsyncRelayCommand(() =>  _setupConfigurator.AddGroupAsync(SelectedItem!), _ => _setupConfigurator.CanAddGroup(SelectedItem));
        AddIncludePatternCommand = new AsyncRelayCommand(() =>  _setupConfigurator.AddIncludePatternAsync(SelectedItem!), _ => _setupConfigurator.CanAddIncludePattern(SelectedItem));
        AddPathCommand = new AsyncRelayCommand(() =>  _setupConfigurator.AddPathAsync(SelectedItem!), _ => _setupConfigurator.CanAddPath(SelectedItem));
        AddSelectorCommand = new AsyncRelayCommand(() =>  _setupConfigurator.AddSelectorAsync(SelectedItem!), _ => _setupConfigurator.CanAddSelector(SelectedItem));
        UpdateCommand = new AsyncRelayCommand(() =>  _setupConfigurator.UpdateAsync(SelectedItem!), _ => _setupConfigurator.CanUpdate(SelectedItem));
        DeleteCommand = new AsyncRelayCommand(() =>  _setupConfigurator.DeleteAsync(SelectedItem!), _ => _setupConfigurator.CanDelete(SelectedItem));
    }

    public async Task OnInititalNavigationAsync(NavigationContext context)
    {
        ConfigurationViewModel = await _configurationViewModelFactory.CreateAsync().ConfigureAwait(true);
        var treeViewItem = await _setupConfigurator.InitializeAsync().ConfigureAwait(true);
        TreeViewItems = new List<TreeViewItem> { treeViewItem };
        
        context.AddParameter(_setupConfigurator);
        
        Refresh();
    }

    public IAsyncCommand AddRootCommand { get; set; }
    public IAsyncCommand AddGroupCommand { get; set; }
    public IAsyncCommand AddIncludePatternCommand { get; set; }
    public IAsyncCommand AddPathCommand { get; set; }
    public IAsyncCommand AddSelectorCommand { get; set; }
    public IAsyncCommand UpdateCommand { get; set; }
    public IAsyncCommand DeleteCommand { get; set; }

    public List<TreeViewItem>? TreeViewItems
    {
        get => Get<List<TreeViewItem>?>();
        set => Set(value);
    }

    public TreeViewItem? SelectedItem
    {
        get => Get<TreeViewItem?>();
        set
        {
            Set(value);
            Refresh();
        }
    }
    
    public ConfigurationWizardViewModel? ConfigurationViewModel
    {
        get => Get<ConfigurationWizardViewModel?>();
        set => Set(value);
    }

    private void Refresh()
    {
        AddRootCommand.RaiseCanExecuteChanged();
        AddIncludePatternCommand.RaiseCanExecuteChanged();
        AddGroupCommand.RaiseCanExecuteChanged();
        AddPathCommand.RaiseCanExecuteChanged();
        AddSelectorCommand.RaiseCanExecuteChanged();
        UpdateCommand.RaiseCanExecuteChanged();
        DeleteCommand.RaiseCanExecuteChanged();
    }
}