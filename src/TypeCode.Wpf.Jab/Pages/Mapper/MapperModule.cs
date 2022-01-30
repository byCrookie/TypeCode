using Autofac;
using TypeCode.Wpf.Jab.Helper.Autofac;

namespace TypeCode.Wpf.Jab.Pages.Mapper;

public class MapperModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<MapperViewModel, MapperView>();

        base.Load(builder);
    }
}