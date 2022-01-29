using Autofac;
using TypeCode.Wpf.Helper.Autofac;

namespace TypeCode.Wpf.Pages.UnitTestDependencyManually
{
    public class UnitTestDependencyManuallyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddViewModelAndView<UnitTestDependencyManuallyViewModel, UnitTestDependencyManuallyView>();

            base.Load(builder);
        }
    }
}