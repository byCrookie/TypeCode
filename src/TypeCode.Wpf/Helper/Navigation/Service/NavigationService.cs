using System.Windows.Controls;
using DependencyInjection.Factory;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Main;

namespace TypeCode.Wpf.Helper.Navigation.Service;

public sealed class NavigationService : INavigationService
{
    private readonly IMainViewProvider _mainViewProvider;
    private readonly IFactory _factory;
    private object? _lastViewModel;

    public NavigationService(IMainViewProvider mainViewProvider, IFactory factory)
    {
        _mainViewProvider = mainViewProvider;
        _factory = factory;
    }

    public async Task NavigateAsync<T>(NavigationContext context) where T : notnull
    {
        await CallNavigatedFromOnLastViewModelAsync(context).ConfigureAwait(true);

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

        if (!_mainViewProvider.MainContentView().NavigationFrame.Navigate(viewInstance))
        {
            throw new ApplicationException($"Navigation to {viewModelType.Name} failed");
        }

        await CallOnNavigatedToOnCurrentViewModelAsync(context, viewModelInstance).ConfigureAwait(true);

        _lastViewModel = viewModelInstance;
    }

    private static async Task CallOnNavigatedToOnCurrentViewModelAsync<T>(NavigationContext context, T viewModelInstance)
    {
        await NavigationCaller.CallInitialNavigateAsync(viewModelInstance, context).ConfigureAwait(true);
        await NavigationCaller.CallNavigateToAsync(viewModelInstance, context).ConfigureAwait(true);
    }

    private Task CallNavigatedFromOnLastViewModelAsync(NavigationContext context)
    {
        return NavigationCaller.CallNavigateFromAsync(_lastViewModel, context);
    }
}