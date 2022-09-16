using System.Windows;
using System.Windows.Controls;
using DependencyInjection.Factory;
using TypeCode.Wpf.Helper.Navigation.Modal.View;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Main;

namespace TypeCode.Wpf.Helper.Navigation.Modal.Service;

public sealed class ModalNavigationService : IModalNavigationService
{
    private readonly IMainViewProvider _mainViewProvider;
    private readonly IFactory<ModalViewModel> _modalViewModelFactory;
    private ModalParameter? _lastModalParameter;

    public ModalNavigationService(IMainViewProvider mainViewProvider, IFactory<ModalViewModel> modalViewModelFactory)
    {
        _mainViewProvider = mainViewProvider;
        _modalViewModelFactory = modalViewModelFactory;
    }

    public async Task OpenModalAsync(ModalParameter modalParameter)
    {
        var viewModelType = typeof(ModalViewModel);
        var viewModelInstance = _modalViewModelFactory.Create();

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

        _mainViewProvider.MainWindow().Main.Opacity = 0.1;
        _mainViewProvider.MainWindow().Main.IsEnabled = false;
        _mainViewProvider.MainWindow().ModalOverlay.Visibility = Visibility.Visible;
        if (!_mainViewProvider.MainWindow().ModalFrame.Navigate(viewInstance))
        {
            throw new ApplicationException($"Navigation to modal {viewModelType.Name} failed");
        }

        var context = new NavigationContext();
        context.AddParameter(modalParameter);
        await CallOnNavigatedToOnCurrentViewModelAsync(context, viewModelInstance).ConfigureAwait(true);

        _lastModalParameter = modalParameter;
    }
    
    public Task CancelAsync()
    {
        _mainViewProvider.MainWindow().Main.Opacity = 1;
        _mainViewProvider.MainWindow().Main.IsEnabled = true;
        _mainViewProvider.MainWindow().ModalOverlay.Visibility = Visibility.Collapsed;

        return _lastModalParameter?.OnCancelAsync.Invoke() ?? Task.CompletedTask;
    }

    public Task OkAsync()
    {
        _mainViewProvider.MainWindow().Main.Opacity = 1;
        _mainViewProvider.MainWindow().Main.IsEnabled = true;
        _mainViewProvider.MainWindow().ModalOverlay.Visibility = Visibility.Collapsed;

        return _lastModalParameter?.OnOkAsync.Invoke() ?? Task.CompletedTask;
    }

    private static Task CallOnNavigatedToOnCurrentViewModelAsync<T>(NavigationContext context, T viewModelInstance)
    {
        return NavigationCaller.CallNavigateToAsync(viewModelInstance, context);
    }
}