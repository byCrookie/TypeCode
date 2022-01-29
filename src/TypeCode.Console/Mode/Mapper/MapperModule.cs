using Autofac;

namespace TypeCode.Console.Mode.Mapper;

internal class MapperModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MapperTypeCodeStrategy>().As<IMapperTypeCodeStrategy>();
        base.Load(builder);
    }
}