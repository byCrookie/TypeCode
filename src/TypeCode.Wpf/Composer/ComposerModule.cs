using Autofac;

namespace TypeCode.Wpf.Composer
{
    public class ComposerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ComposerViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}