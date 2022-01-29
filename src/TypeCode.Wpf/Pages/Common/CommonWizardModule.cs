using Autofac;
using TypeCode.Wpf.Helper.Autofac;
using TypeCode.Wpf.Pages.Common.Configuration;

namespace TypeCode.Wpf.Pages.Common;

public class CommonWizardModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<ConfigurationWizardViewModel, ConfigurationWizardView>();

        base.Load(builder);
    }
}