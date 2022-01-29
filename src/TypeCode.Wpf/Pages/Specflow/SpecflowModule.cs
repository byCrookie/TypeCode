using Autofac;
using TypeCode.Wpf.Helper.Autofac;

namespace TypeCode.Wpf.Pages.Specflow
{
    public class SpecflowModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddViewModelAndView<SpecflowViewModel, SpecflowView>();

            base.Load(builder);
        }
    }
}