using System.Windows.Controls;
using Framework.DependencyInjection.Factory;
using TypeCode.Wpf.Helper.Autofac;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Main;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

public class WizardBuilder : IWizardBuilder
{
    private readonly IFactory _factory;

    private readonly Wizard _wizard;

    public WizardBuilder(IFactory factory, MainWindow mainWindow, NavigationContext navigationContext)
    {
        _factory = factory;

        _wizard = new Wizard(new WizardBuilderOptions(
                navigationContext,
                mainWindow.WizardFrame,
                mainWindow.Main,
                mainWindow.WizardOverlay),
            CreateInstances<WizardViewModel>()
        );
    }

    public IWizardBuilder Then<TViewModel>(Action<IWizardParameterBuilder, NavigationContext>? configureParameter = null)
        where TViewModel : notnull
    {
        var parameterBuilder = _factory.Create<IWizardParameterBuilder>();
        configureParameter?.Invoke(parameterBuilder, _wizard.NavigationContext);
        var parameter = parameterBuilder.Build();

        var stepConfiguration = new WizardStepConfiguration(
            parameter.AfterAction,
            parameter.BeforeAction,
            parameter.AllowBack,
            parameter.AllowNext,
            CreateInstances<TViewModel>()
        );

        if (_wizard.StepConfigurations.LastOrDefault() is not null)
        {
            stepConfiguration.LastStep = _wizard.StepConfigurations.Last();
            stepConfiguration.LastStep.NextStep = stepConfiguration;
        }

        _wizard.StepConfigurations.Add(stepConfiguration);

        return this;
    }

    public IWizardBuilder FinishAsync(Func<NavigationContext, Task> completedAction)
    {
        _wizard.CompletedAction = completedAction;
        return this;
    }

    public IWizardBuilder PublishAsync<TEvent>()
    {
        _wizard.CompletedEvent = typeof(TEvent);
        return this;
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

    public Wizard Build()
    {
        return _wizard;
    }
}