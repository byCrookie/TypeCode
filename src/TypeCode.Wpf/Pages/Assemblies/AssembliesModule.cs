using Autofac;
using TypeCode.Wpf.Helper.Autofac;

namespace TypeCode.Wpf.Pages.Assemblies
{
    public class AssemblyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddViewModelAndView<AssemblyViewModel, AssemblyView>();

            base.Load(builder);
        }
    }
}