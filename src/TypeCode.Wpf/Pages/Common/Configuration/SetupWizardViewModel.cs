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

        AddRootCommand = new AsyncRelayCommand(AddRootAsync, CanAddRoot);
        AddGroupCommand = new AsyncRelayCommand(AddGroupAsync, CanAddGroup);
        AddIncludePatternCommand = new AsyncRelayCommand(AddIncludePatternAsync, CanAddIncludePattern);
        AddPathCommand = new AsyncRelayCommand(AddPathAsync, CanAddPath);
        AddSelectorCommand = new AsyncRelayCommand(AddSelectorAsync, CanAddSelector);
    }

    private bool CanAddRoot(object? arg)
    {
        return _setupConfigurator.CanAddRoot(SelectedItem);
    }

    private Task AddRootAsync()
    {
        return _setupConfigurator.AddRootAsync(SelectedItem!);
    }

    private bool CanAddGroup(object? arg)
    {
        return _setupConfigurator.CanAddGroup(SelectedItem);
    }

    private Task AddGroupAsync()
    {
        return _setupConfigurator.AddGroupAsync(SelectedItem!);
    }
    
    private bool CanAddIncludePattern(object? arg)
    {
        return _setupConfigurator.CanAddIncludePattern(SelectedItem);
    }

    private Task AddIncludePatternAsync()
    {
        return _setupConfigurator.AddIncludePatternAsync(SelectedItem!);
    }

    private bool CanAddPath(object? arg)
    {
        return _setupConfigurator.CanAddPath(SelectedItem);
    }

    private Task AddPathAsync()
    {
        return _setupConfigurator.AddPathAsync(SelectedItem!);
    }

    private bool CanAddSelector(object? arg)
    {
        return _setupConfigurator.CanAddSelector(SelectedItem);

    }

    private Task AddSelectorAsync()
    {
        return _setupConfigurator.AddSelectorAsync(SelectedItem!);
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
    }
}