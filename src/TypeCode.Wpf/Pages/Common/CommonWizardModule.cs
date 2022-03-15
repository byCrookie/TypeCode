using Autofac;
using TypeCode.Wpf.Helper.Autofac;
using TypeCode.Wpf.Pages.Common.Configuration;
using TypeCode.Wpf.Pages.Common.Configuration.AssemblyRoot;

namespace TypeCode.Wpf.Pages.Common;

public class CommonWizardModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<ConfigurationWizardViewModel, ConfigurationWizardView>();
        builder.AddViewModelAndView<SetupWizardViewModel, SetupWizardView>();
        builder.AddViewModelAndView<AssemblyRootWizardViewModel, AssemblyRootWizardView>();

        base.Load(builder);
    }
}