using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Framework.Autofac.Factory;
using TypeCode.Wpf.Helper.Autofac;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex
{
    public class WizardBuilder : IWizardBuilder
    {
        private readonly IFactory _factory;

        private Wizard _wizard;

        public WizardBuilder(IFactory factory)
        {
            _factory = factory;
        }

        public IWizardBuilder Init(NavigationContext navigationContext, Frame navigationFrame, UIElement content, UIElement wizardOverlay)
        {
            _wizard = new Wizard
            {
                NavigationContext = navigationContext,
                NavigationFrame = navigationFrame,
                Content = content,
                WizardOverlay = wizardOverlay,
                WizardInstances = CreateInstances<WizardViewModel>()
            };

            return this;
        }

        public IWizardBuilder Then<TViewModel>(Action<WizardParameterBuilder, NavigationContext> configureParameter = null)
        {
            var parameterBuilder = new WizardParameterBuilder();
            configureParameter?.Invoke(parameterBuilder, _wizard.NavigationContext);
            var parameter = parameterBuilder.Build();

            var stepConfiguration = new WizardStepConfiguration
            {
                AfterAction = parameter.AfterAction,
                BeforeAction = parameter.BeforeAction,
                AllowBack = parameter.AllowBack,
                AllowNext = parameter.AllowNext,
                Instances = CreateInstances<TViewModel>()
            };

            if (_wizard.StepConfigurations.LastOrDefault() is not null)
            {
                stepConfiguration.LastStep = _wizard.StepConfigurations.Last();
                stepConfiguration.LastStep.NextStep = stepConfiguration;
            }
            
            _wizard.StepConfigurations.Add(stepConfiguration);

            return this;
        }

        public IWizardBuilder Finish(Func<NavigationContext, Task> completedAction)
        {
            _wizard.CompletedAction = completedAction;
            return this;
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

        public Wizard Build()
        {
            var wizard = _wizard;
            _wizard = null;
            wizard.CurrentStepConfiguration = wizard.StepConfigurations.FirstOrDefault();
            return wizard;
        }
    }
}