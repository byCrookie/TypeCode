using Autofac;

namespace TypeCode.Wpf.Pages.TypeSelection
{
    public class TypeSelectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TypeSelectionViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}