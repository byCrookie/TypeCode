using Autofac;
using TypeCode.Wpf.Jab.Helper.Autofac;

namespace TypeCode.Wpf.Jab.Pages.Assemblies;

public class AssemblyModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<AssemblyViewModel, AssemblyView>();

        base.Load(builder);
    }
}