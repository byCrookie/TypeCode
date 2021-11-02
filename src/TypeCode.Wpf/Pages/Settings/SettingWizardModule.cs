using Autofac;
using TypeCode.Wpf.Pages.Settings.First;
using TypeCode.Wpf.Pages.Settings.Second;

namespace TypeCode.Wpf.Pages.Settings
{
    public class SettingWizardModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SettingFirstWizardViewModel>().AsSelf();
            builder.RegisterType<SettingSecondWizardViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}