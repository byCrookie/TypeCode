using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Framework.Jab.Jab.Factory;
using TypeCode.Wpf.Helper.Autofac;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Steps.WizardEndStep;
using TypeCode.Wpf.Helper.Navigation.Wizard.View;
using Workflow;

namespace TypeCode.Wpf.Helper.Navigation.Wizard
{
    public class WizardNavigator : IWizardNavigator
    {
        private readonly IFactory _factory;

        public WizardNavigator(IFactory factory)
        {
            _factory = factory;
        }

        public async Task<TContext> StartAsync<TContext>(WizardNavigatorParameter parameter, TContext context, IWorkflow<TContext> wizardFlow) where TContext : WizardContext
        {
            parameter.MainContentControl.Opacity = 0.1;
            parameter.MainContentControl.IsEnabled = false;
            parameter.WizardOverlayControl.Visibility = Visibility.Visible;

            context.NavigationContext.AddParameter(parameter);
            context.NavigationContext.AddParameter("WizardContext", context);
            var endStep = _factory.Create<IWizardEndStep<WizardContext, WizardEndStepOptions>>();
            endStep.SetOptions(new WizardEndStepOptions());
            wizardFlow.AddStep(endStep);

            context.Wizard = CreateInstances<WizardSimpleViewModel>();
            context.NavigationContext.GetParameter<WizardNavigatorParameter>().NavigationFrame.Navigate(context.Wizard.ViewInstance);

            var wizardContext = await wizardFlow.RunAsync(context).ConfigureAwait(true);
            return wizardContext;
        }

        public async Task NextOrNewAsync<TViewModel>(WizardContext context)
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
        
        private InstanceResult CreateInstances<T>()
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

            return new InstanceResult
            {
                ViewType = viewType,
                ViewInstance = viewInstance,
                ViewModelInstance = viewModelInstance,
                ViewModelType = viewModelType
            };
        }
    }
}