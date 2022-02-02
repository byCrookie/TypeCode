using System.Windows;
using System.Windows.Controls;
using Framework.DependencyInjection.Factory;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Modal.View;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Main;

namespace TypeCode.Wpf.Helper.Navigation.Modal.Service;

public class ModalNavigationService : IModalNavigationService
{
    private readonly MainWindow _mainWindow;
    private readonly IFactory _factory;
    private ModalParameter? _lastModalParameter;

    public ModalNavigationService(MainWindow mainWindow, IFactory factory)
    {
        _mainWindow = mainWindow;
        _factory = factory;
    }

    public async Task OpenModalAsync(ModalParameter modalParameter)
    {
        var viewModelType = typeof(ModalViewModel);
        var viewModelInstance = _factory.Create<ModalViewModel>();

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

        _mainWindow.Main.Opacity = 0.1;
        _mainWindow.Main.IsEnabled = false;
        _mainWindow.ModalOverlay.Visibility = Visibility.Visible;
        if (!_mainWindow.ModalFrame.Navigate(viewInstance))
        {
            throw new ApplicationException($"Navigation to modal {viewModelType.Name} failed");
        }

        var context = new NavigationContext();
        context.AddParameter(modalParameter);
        await CallOnNavigatedToOnCurrentViewModelAsync(context, viewModelInstance).ConfigureAwait(true);

        _lastModalParameter = modalParameter;
    }

    public Task CloseModalAsync()
    {
        _mainWindow.Main.Opacity = 1;
        _mainWindow.Main.IsEnabled = true;
        _mainWindow.ModalOverlay.Visibility = Visibility.Collapsed;

        return _lastModalParameter?.OnCloseAsync.Invoke() ?? Task.CompletedTask;
    }

    private static Task CallOnNavigatedToOnCurrentViewModelAsync<T>(NavigationContext context, T viewModelInstance)
    {
        if (viewModelInstance is IAsyncNavigatedTo asyncNavigatedTo)
        {
            return asyncNavigatedTo.OnNavigatedToAsync(context);
        }
            
        return Task.CompletedTask;
    }
}