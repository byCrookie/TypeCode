using Autofac;

namespace TypeCode.Business.Mode.Selection
{
    internal class SelectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(SelectionStep<,>)).As(typeof(ISelectionStep<,>));

            base.Load(builder);
        }
    }
}