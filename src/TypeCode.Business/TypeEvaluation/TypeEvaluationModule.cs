using Autofac;

namespace TypeCode.Business.TypeEvaluation;

internal class TypeEvaluationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TypeProvider>().As<ITypeProvider>().SingleInstance();
            
        base.Load(builder);
    }
}