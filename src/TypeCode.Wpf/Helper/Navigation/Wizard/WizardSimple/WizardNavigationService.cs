using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Framework.DependencyInjection.Factory;
using TypeCode.Business.Format;
using TypeCode.Wpf.Application.Boot.SetupWpfApplication;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

public class WizardNavigationService : IWizardNavigationService
{
    private readonly IFactory _factory;
    private readonly IWpfWindowProvider _windowProvider;
    private static bool _isOpen;
    private NavigationContext _lastNavigationContext;
    private object _lastViewModel;

    public WizardNavigationService(IFactory factory, IWpfWindowProvider windowProvider)
    {
        _factory = factory;
        _windowProvider = windowProvider;
    }

    public async Task<T> OpenWizardAsync<T>(WizardParameter<T> parameter, NavigationContext context = null)
    {
        // Wizard

        var wizardViewModelType = typeof(WizardSimpleViewModel<T>);
        var wizardViewModelInstance = _factory.Create<WizardSimpleViewModel<T>>();

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

        // Nav

        var mainWindow = _windowProvider.Get().MainWindow;
        mainWindow.Main.Opacity = 0.1;
        mainWindow.Main.IsEnabled = false;
        mainWindow.WizardOverlay.Visibility = Visibility.Visible;
        if (!mainWindow.WizardFrame.Navigate(wizardViewInstance))
        {
            throw new ApplicationException($"Navigation to wizard {wizardViewModelType.Name} failed");
        }

        var navigationContextToUse = context ?? new NavigationContext();
        navigationContextToUse.AddParameter(parameter);
        navigationContextToUse.AddParameter("View", viewInstance);
        await CallOnNavigatedToOnViewModelAsync(navigationContextToUse, wizardViewModelInstance).ConfigureAwait(true);
        await CallOnNavigatedToOnViewModelAsync(navigationContextToUse, viewModelInstance).ConfigureAwait(true);
            
        _lastNavigationContext = navigationContextToUse;
        _lastViewModel = viewModelInstance;
            
        _isOpen = true;
            
        while (_isOpen)
        {
            await Task.Delay(25).ConfigureAwait(true);
        }
            
        return viewModelInstance;
    }

    public async Task CloseWizardAsync<T>()
    {
        var mainWindow = _windowProvider.Get().MainWindow;
        
        mainWindow.Main.Opacity = 1;
        mainWindow.Main.IsEnabled = true;
        mainWindow.WizardOverlay.Visibility = Visibility.Collapsed;

        await CallNavigatedFromOnLastViewModelAsync(_lastNavigationContext).ConfigureAwait(true);

        _isOpen = false;
    }

    private Task CallNavigatedFromOnLastViewModelAsync(NavigationContext context)
    {
        return _lastViewModel is IAsyncNavigatedFrom asyncNavigatedFrom
            ? asyncNavigatedFrom.OnNavigatedFromAsync(context ?? new NavigationContext())
            : Task.CompletedTask;
    }

    private static Task CallOnNavigatedToOnViewModelAsync<T>(NavigationContext context, T viewModelInstance)
    {
        if (viewModelInstance is IAsyncNavigatedTo asyncNavigatedTo)
        {
            return asyncNavigatedTo.OnNavigatedToAsync(context ?? new NavigationContext());
        }

        return Task.CompletedTask;
    }
}