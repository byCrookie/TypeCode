using System.Windows;
using System.Windows.Controls;
using Framework.DependencyInjection.Factory;
using TypeCode.Wpf.Helper.Autofac;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public class WizardNavigator : IWizardNavigator
{
    private readonly IFactory _factory;

    public WizardNavigator(IFactory factory)
    {
        _factory = factory;
    }
    
    public async Task NextOrNewAsync<TViewModel>(WizardContext context) where TViewModel : notnull
    {
        var nextEntry = context.NavigationJournal.GetOrAddNextEntry(CreateInstances<TViewModel>());
        context.NavigationContext.AddOrUpdateParameter("View", nextEntry.InstanceResult.ViewInstance);
        await CallOnNavigatedToOnCurrentViewModelAsync(context.Wizard.ViewModelInstance, context.NavigationContext).ConfigureAwait(true);
        await CallOnNavigatedToOnCurrentViewModelAsync(nextEntry.InstanceResult.ViewModelInstance, context.NavigationContext).ConfigureAwait(true);
        nextEntry.IsOpen = true;

        while (nextEntry.IsOpen)
        {
            await Task.Delay(25).ConfigureAwait(true);
        }
    }

    public async Task BackAsync(WizardContext context)
    {
        var backEntry = context.NavigationJournal.GetBackEntry();
        context.NavigationContext.AddOrUpdateParameter("View", backEntry.InstanceResult.ViewInstance);
        await CallOnNavigatedToOnCurrentViewModelAsync(context.Wizard.ViewModelInstance, context.NavigationContext).ConfigureAwait(true);
        await CallOnNavigatedToOnCurrentViewModelAsync(backEntry.InstanceResult.ViewModelInstance, context.NavigationContext).ConfigureAwait(true);

        backEntry.IsOpen = true;

        while (backEntry.IsOpen)
        {
            await Task.Delay(25).ConfigureAwait(true);
        }
    }

    public async Task CloseAsync(WizardContext context)
    {
        await CloseCurrentAsync(context).ConfigureAwait(true);
            
        await CallNavigatedFromViewModelAsync(context.Wizard.ViewModelInstance, context.NavigationContext).ConfigureAwait(true);

        var wizardParameter = context.NavigationContext.GetParameter<WizardNavigatorParameter>();
        wizardParameter.MainContentControl.Opacity = 1;
        wizardParameter.MainContentControl.IsEnabled = true;
        wizardParameter.WizardOverlayControl.Visibility = Visibility.Collapsed;
    }

    public async Task CloseCurrentAsync(WizardContext context)
    {
        var currentEntry = context.NavigationJournal.GetCurrentEntry();
        await CallNavigatedFromViewModelAsync(currentEntry.InstanceResult.ViewModelInstance, context.NavigationContext).ConfigureAwait(true);
        currentEntry.IsOpen = false;
    }

    private static Task CallNavigatedFromViewModelAsync(object viewModelInstance, NavigationContext context)
    {
        if (viewModelInstance is IAsyncNavigatedFrom asyncNavigatedFrom)
        {
            return asyncNavigatedFrom.OnNavigatedFromAsync(context);
        }

        return Task.CompletedTask;
    }

    private static Task CallOnNavigatedToOnCurrentViewModelAsync(object viewModelInstance, NavigationContext context)
    {
        if (viewModelInstance is IAsyncNavigatedTo asyncNavigatedTo)
        {
            return asyncNavigatedTo.OnNavigatedToAsync(context);
        }

        return Task.CompletedTask;
    }
        
    private InstanceResult CreateInstances<T>() where T : notnull
    {
        var viewModelType = typeof(T);
        var viewModelInstance = _factory.Create<T>();

        if (viewModelInstance is null)
        {
            throw new ApplicationException($"ViewModel of {viewModelType.Name} not found");
        }

        var viewType = viewModelType.GetViewType();
            
        if (viewType is null || _factory.Create(viewType) is not UserControl viewInstance)
        {
            throw new ApplicationException($"View of {viewModelType.Name} not found");
        }

        viewInstance.DataContext = viewModelInstance;

        return new InstanceResult(viewType, viewInstance, viewModelType, viewModelInstance);
    }
}