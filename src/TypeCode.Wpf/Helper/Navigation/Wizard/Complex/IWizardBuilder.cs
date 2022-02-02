using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

public interface IWizardBuilder
{
    IWizardAfterInitialBuilder Then<TViewModel>(Action<IWizardParameterBuilder, NavigationContext>? configureParameter = null) where TViewModel : notnull;
}