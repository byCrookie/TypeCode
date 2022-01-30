using System.Threading.Tasks;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Steps.WizardStep;

internal class WizardStep<TWizardPage, TContext, TOptions> :
    IWizardStep<TWizardPage, TContext, TOptions>
    where TContext : WizardContext
{
    private readonly IWizardNavigator _wizardNavigator;

    public WizardStep(IWizardNavigator wizardNavigator)
    {
        _wizardNavigator = wizardNavigator;
    }
        
    private WizardStepOptions _options;

    public Task ExecuteAsync(TContext context)
    {
        return _wizardNavigator.NextOrNewAsync<TWizardPage>(context);
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }

    public void SetOptions(TOptions options)
    {
        _options = options as WizardStepOptions;
    }
}