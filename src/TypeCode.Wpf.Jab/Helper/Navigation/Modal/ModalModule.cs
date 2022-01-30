using Autofac;
using TypeCode.Wpf.Jab.Helper.Autofac;
using TypeCode.Wpf.Jab.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Jab.Helper.Navigation.Modal.View;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Modal;

public class ModalModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ModalNavigationService>().As<IModalNavigationService>().SingleInstance();
            
        builder.AddViewModelAndView<ModalViewModel, ModalView>();
            
        base.Load(builder);
    }
}