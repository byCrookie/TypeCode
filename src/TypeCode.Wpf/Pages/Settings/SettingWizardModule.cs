using Autofac;
using TypeCode.Wpf.Helper.Autofac;
using TypeCode.Wpf.Pages.Settings.First;
using TypeCode.Wpf.Pages.Settings.Second;

namespace TypeCode.Wpf.Pages.Settings
{
    public class SettingWizardModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddViewModelAndView<SettingFirstWizardViewModel, SettingFirstWizardView>();
            builder.AddViewModelAndView<SettingSecondWizardViewModel, SettingSecondWizardView>();

            base.Load(builder);
        }
    }
}