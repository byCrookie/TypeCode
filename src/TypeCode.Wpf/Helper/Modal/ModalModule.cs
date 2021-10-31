using Autofac;

namespace TypeCode.Wpf.Helper.Modal
{
    public class ModalModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ModalViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}