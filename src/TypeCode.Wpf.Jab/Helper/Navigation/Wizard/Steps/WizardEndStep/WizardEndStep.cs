using System.Threading.Tasks;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Steps.WizardEndStep;

internal class WizardEndStep<TContext, TOptions> :
    IWizardEndStep<TContext, TOptions>
    where TContext : WizardContext
{
    private readonly IWizardNavigator _wizardNavigator;

    public WizardEndStep(IWizardNavigator wizardNavigator)
    {
        _wizardNavigator = wizardNavigator;
    }
        
    private WizardEndStepOptions _options;

    public Task ExecuteAsync(TContext context)
    {
        return _wizardNavigator.CloseAsync(context);
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }

    public void SetOptions(TOptions options)
    {
        _options = options as WizardEndStepOptions;
    }
}