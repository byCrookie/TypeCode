using Autofac;
using TypeCode.Wpf.Jab.Helper.Autofac;
using TypeCode.Wpf.Jab.Pages.Common.Configuration;

namespace TypeCode.Wpf.Jab.Pages.Common;

public class CommonWizardModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<ConfigurationWizardViewModel, ConfigurationWizardView>();

        base.Load(builder);
    }
}