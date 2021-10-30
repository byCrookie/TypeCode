using Autofac;

namespace TypeCode.Wpf.Mapper
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MapperViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}