using Autofac;
using TypeCode.Wpf.Helper.Autofac;

namespace TypeCode.Wpf.Pages.Mapper
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddViewModelAndView<MapperViewModel, MapperView>();

            base.Load(builder);
        }
    }
}