using Autofac;

namespace TypeCode.Business.Mode.MultipleTypes
{
    internal class MultipleTypesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(MultipleTypeSelectionStep<>)).As(typeof(IMultipleTypeSelectionStep<>));

            base.Load(builder);
        }
    }
}