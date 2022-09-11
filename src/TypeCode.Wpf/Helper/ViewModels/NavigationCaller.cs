using System.Collections;
using System.Reflection;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.ViewModels;

public static class NavigationCaller
{
    public static async Task CallInitialNavigateAsync(object? viewModel, NavigationContext navigationContext)
    {
        switch (viewModel)
        {
            case null:
                return;
            case IAsyncInitialNavigated asyncNavigatedTo:

                if (viewModel is ViewModelBase { Initialized: false } uninitializedViewModelBase)
                {
                    await asyncNavigatedTo.OnInititalNavigationAsync(navigationContext).ConfigureAwait(true);
                    uninitializedViewModelBase.Initialized = true;
                }
                else if (viewModel is not ViewModelBase)
                {
                    throw new ArgumentException($"{nameof(IAsyncInitialNavigated)} only works on {nameof(ViewModelBase)}");
                }

                break;
        }

        var fields = viewModel.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (var property in fields)
        {
            if (property.GetCustomAttribute<ChildViewModelAttribute>() is not null)
            {
                var fieldValue = property.GetValue(viewModel);

                switch (fieldValue)
                {
                    case null:
                        continue;
                    case IEnumerable values:
                    {
                        foreach (var value in values)
                        {
                            await CallInitialNavigateAsync(value, navigationContext).ConfigureAwait(true);
                        }

                        break;
                    }
                    default:
                        await CallInitialNavigateAsync(fieldValue, navigationContext).ConfigureAwait(true);
                        break;
                }
            }
        }
        
        if (viewModel is ViewModelBase viewModelBase)
        {
            viewModelBase.OnAllPropertiesChanged();
            viewModelBase.ValidateAllProperties();
        }
    }

    public static async Task CallNavigateToAsync(object? viewModel, NavigationContext navigationContext)
    {
        switch (viewModel)
        {
            case null:
                return;
            case IAsyncNavigatedTo asyncNavigatedTo:
                await asyncNavigatedTo.OnNavigatedToAsync(navigationContext).ConfigureAwait(true);
                break;
        }

        var fields = viewModel.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (var property in fields)
        {
            if (property.GetCustomAttribute<ChildViewModelAttribute>() is not null)
            {
                var fieldValue = property.GetValue(viewModel) ?? throw new Exception($"Can not get value of field {property.Name} of type {viewModel.GetType().FullName}");

                if (fieldValue is IEnumerable values)
                {
                    foreach (var value in values)
                    {
                        await CallNavigateToAsync(value, navigationContext).ConfigureAwait(true);
                    }
                }
                else
                {
                    await CallNavigateToAsync(fieldValue, navigationContext).ConfigureAwait(true);
                }
            }
        }
        
        if (viewModel is ViewModelBase viewModelBase)
        {
            viewModelBase.OnAllPropertiesChanged();
            viewModelBase.ValidateAllProperties();
        }
    }

    public static async Task CallNavigateFromAsync(object? viewModel, NavigationContext navigationContext)
    {
        switch (viewModel)
        {
            case null:
                return;
            case IAsyncNavigatedFrom asyncNavigatedFrom:
                await asyncNavigatedFrom.OnNavigatedFromAsync(navigationContext).ConfigureAwait(true);
                break;
        }

        var fields = viewModel.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<ChildViewModelAttribute>() is not null)
            {
                var fieldValue = field.GetValue(viewModel) ?? throw new Exception($"Can not get value of field {field.Name} of type {viewModel.GetType().FullName}");
                if (fieldValue is IEnumerable values)
                {
                    foreach (var value in values)
                    {
                        await CallNavigateFromAsync(value, navigationContext).ConfigureAwait(true);
                    }
                }
                else
                {
                    await CallNavigateFromAsync(fieldValue, navigationContext).ConfigureAwait(true);
                }
            }
        }
    }
}