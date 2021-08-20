using Autofac;
using TypeCode.Business.Mode.Mapper.Style;

namespace TypeCode.Business.Mode.Mapper
{
    internal class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MapperTypeCodeStrategy>().As<IMapperTypeCodeStrategy>();
            builder.RegisterType<ExistingMapperStyleStrategy>().As<IExistingMapperStyleStrategy>();
            builder.RegisterType<NewMapperStyleStrategy>().As<INewMapperStyleStrategy>();
            builder.RegisterType<MapperStyleComposer>().As<IMapperStyleComposer>();
            base.Load(builder);
        }
    }
}