using System.Collections;
using System.Reflection;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.ViewModels;

public static class NavigationCaller
{
    public static async Task CallNavigateToAsync(object? viewModel, NavigationContext navigationContext)
    {
        switch (viewModel)
        {
            case null:
                return;
            case IAsyncNavigatedTo asyncNavigatedTo:
                await asyncNavigatedTo.OnNavigatedToAsync(navigationContext);
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
                        await CallNavigateToAsync(value, navigationContext);
                    }
                }
                else
                {
                    await CallNavigateToAsync(fieldValue, navigationContext);
                }
            }
        }
    }
    
    public static async Task CallNavigateFromAsync(object? viewModel, NavigationContext navigationContext)
    {
        switch (viewModel)
        {
            case null:
                return;
            case IAsyncNavigatedFrom asyncNavigatedFrom:
                await asyncNavigatedFrom.OnNavigatedFromAsync(navigationContext);
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
                        await CallNavigateFromAsync(value, navigationContext);
                    }
                }
                else
                {
                    await CallNavigateFromAsync(fieldValue, navigationContext);
                }
            }
        }
    }
}