using System.Windows;
using System.Windows.Controls;
using DependencyInjection.Factory;
using TypeCode.Business.Format;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Main;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

public class WizardNavigationService : IWizardNavigationService
{
    private readonly IMainViewProvider _mainViewProvider;
    private readonly IFactory _factory;
    private readonly IEventAggregator _eventAggregator;
    private static bool _isOpen;
    private NavigationContext? _lastNavigationContext;
    private object? _lastViewModel;

    public WizardNavigationService(IMainViewProvider mainViewProvider, IFactory factory, IEventAggregator eventAggregator)
    {
        _mainViewProvider = mainViewProvider;
        _factory = factory;
        _eventAggregator = eventAggregator;
    }

    public async Task<T> OpenWizardAsync<T>(WizardParameter<T> parameter, NavigationContext context) where T : notnull
    {
        // Wizard

        var wizardViewModelType = typeof(WizardSimpleViewModel<T>);
        var wizardViewModelInstance = new WizardSimpleViewModel<T>(this, _eventAggregator);

        if (wizardViewModelInstance is null)
        {
            throw new ApplicationException($"ViewModel of {wizardViewModelType.Name} not found");
        }

        var wizardViewName = NameBuilder.GetNameWithoutGeneric(wizardViewModelType)[..^"Model".Length];
        var wizardViewType = Type.GetType($"{wizardViewModelType.Namespace}.{wizardViewName}");

        if (wizardViewType is null || Activator.CreateInstance(wizardViewType) is not UserControl wizardViewInstance)
        {
            throw new ApplicationException($"View of {wizardViewModelType.Name} not found");
        }

        wizardViewInstance.DataContext = wizardViewModelInstance;

        // Wizard-Page

        var viewModelType = typeof(T);
        var viewModelInstance = _factory.Create<T>();

        if (viewModelInstance is null)
        {
            throw new ApplicationException($"ViewModel of {viewModelType.Name} not found");
        }

        var viewName = viewModelType.Name[..^"Model".Length];
        var viewType = Type.GetType($"{viewModelType.Namespace}.{viewName}");

        if (viewType is null || Activator.CreateInstance(viewType) is not UserControl viewInstance)
        {
            throw new ApplicationException($"View of {viewModelType.Name} not found");
        }

        viewInstance.DataContext = viewModelInstance;

        await parameter.InitializeAsync(viewModelInstance).ConfigureAwait(true);
        
        // Nav

        _mainViewProvider.MainWindow().Main.Opacity = 0.1;
        _mainViewProvider.MainWindow().Main.IsEnabled = false;
        _mainViewProvider.MainWindow().WizardOverlay.Opacity = 0.1;
        _mainViewProvider.MainWindow().WizardOverlay.IsEnabled = false;
        _mainViewProvider.MainWindow().ModalOverlay.Visibility = Visibility.Visible;
        if (!_mainViewProvider.MainWindow().ModalFrame.Navigate(wizardViewInstance))
        {
            throw new ApplicationException($"Navigation to wizard {wizardViewModelType.Name} failed");
        }

        context.AddParameter(parameter);
        context.AddParameter("View", viewInstance);
        context.AddParameter("ViewModel", viewModelInstance);
        await CallOnNavigatedToOnViewModelAsync(context, wizardViewModelInstance).ConfigureAwait(true);
        await CallOnNavigatedToOnViewModelAsync(context, viewModelInstance).ConfigureAwait(true);

        _lastNavigationContext = context;
        _lastViewModel = viewModelInstance;

        _isOpen = true;

        while (_isOpen)
        {
            await Task.Delay(25).ConfigureAwait(true);
        }

        return viewModelInstance;
    }

    public async Task SaveWizardAsync<T>() where T : notnull
    {
        _mainViewProvider.MainWindow().Main.Opacity = 1;
        _mainViewProvider.MainWindow().Main.IsEnabled = true;
        _mainViewProvider.MainWindow().WizardOverlay.Opacity = 1;
        _mainViewProvider.MainWindow().WizardOverlay.IsEnabled = true;
        _mainViewProvider.MainWindow().ModalOverlay.Visibility = Visibility.Collapsed;

        if (_lastNavigationContext is null)
        {
            throw new ArgumentNullException($"{nameof(NavigationContext)} is not set");
        }

        await CallNavigatedFromOnLastViewModelAsync(_lastNavigationContext).ConfigureAwait(true);

        _isOpen = false;

        var parameter = _lastNavigationContext.GetParameter<WizardParameter<T>>();
        await parameter.OnSaveAsync((T)_lastViewModel!, _lastNavigationContext).ConfigureAwait(true);
    }
    
    public async Task CloseWizardAsync<T>() where T : notnull
    {
        _mainViewProvider.MainWindow().Main.Opacity = 1;
        _mainViewProvider.MainWindow().Main.IsEnabled = true;
        _mainViewProvider.MainWindow().WizardOverlay.Opacity = 1;
        _mainViewProvider.MainWindow().WizardOverlay.IsEnabled = true;
        _mainViewProvider.MainWindow().ModalOverlay.Visibility = Visibility.Collapsed;

        if (_lastNavigationContext is null)
        {
            throw new ArgumentNullException($"{nameof(NavigationContext)} is not set");
        }

        await CallNavigatedFromOnLastViewModelAsync(_lastNavigationContext).ConfigureAwait(true);

        _isOpen = false;

        var parameter = _lastNavigationContext.GetParameter<WizardParameter<T>>();
        await parameter.OnCancelAsync((T)_lastViewModel!, _lastNavigationContext).ConfigureAwait(true);
    }

    private Task CallNavigatedFromOnLastViewModelAsync(NavigationContext context)
    {
        return _lastViewModel is IAsyncNavigatedFrom asyncNavigatedFrom
            ? asyncNavigatedFrom.OnNavigatedFromAsync(context)
            : Task.CompletedTask;
    }

    private static Task CallOnNavigatedToOnViewModelAsync<T>(NavigationContext context, T viewModelInstance)
    {
        if (viewModelInstance is IAsyncNavigatedTo asyncNavigatedTo)
        {
            return asyncNavigatedTo.OnNavigatedToAsync(context);
        }

        return Task.CompletedTask;
    }
}