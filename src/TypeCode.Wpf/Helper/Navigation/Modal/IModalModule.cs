using Jab;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Helper.Navigation.Modal.View;

namespace TypeCode.Wpf.Helper.Navigation.Modal;

[ServiceProviderModule]
[Singleton(typeof(IModalNavigationService), typeof(ModalNavigationService))]
[Transient(typeof(ModalView))]
[Transient(typeof(ModalViewModel))]
public interface IModalModule
{
}