using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using Framework.DependencyInjection.Factory;
using TypeCode.Wpf.Jab.Helper.Navigation.Contract;
using TypeCode.Wpf.Jab.Main.Content;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Service;

public class NavigationService : INavigationService
{
    private readonly MainContentView _mainContentView;
    private readonly IFactory _factory;
    private object _lastViewModel;

    public NavigationService(MainContentView mainContentView, IFactory factory)
    {
        _mainContentView = mainContentView;
        _factory = factory;
    }

    public async Task NavigateAsync<T>(NavigationContext context = null)
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

        if (!_mainContentView.NavigationFrame.Navigate(viewInstance))
        {
            throw new ApplicationException($"Navigation to {viewModelType.Name} failed");
        }

        await CallOnNavigatedToOnCurrentViewModelAsync(context, viewModelInstance).ConfigureAwait(true);
            
        _lastViewModel = viewModelInstance;
    }
        
    private static Task CallOnNavigatedToOnCurrentViewModelAsync<T>(NavigationContext context, T viewModelInstance)
    {
        if (viewModelInstance is IAsyncNavigatedTo asyncNavigatedTo)
        {
            return asyncNavigatedTo.OnNavigatedToAsync(context ?? new NavigationContext());
        }
            
        return Task.CompletedTask;
    }

    private Task CallNavigatedFromOnLastViewModelAsync(NavigationContext context)
    {
        return _lastViewModel is IAsyncNavigatedFrom asyncNavigatedFrom
            ? asyncNavigatedFrom.OnNavigatedFromAsync(context ?? new NavigationContext())
            : Task.CompletedTask;
    }
}