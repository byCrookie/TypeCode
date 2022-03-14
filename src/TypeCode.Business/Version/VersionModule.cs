using Autofac;

namespace TypeCode.Business.Version;

internal class VersionModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<VersionEvaluator>().As<IVersionEvaluator>();

        base.Load(builder);
    }
}