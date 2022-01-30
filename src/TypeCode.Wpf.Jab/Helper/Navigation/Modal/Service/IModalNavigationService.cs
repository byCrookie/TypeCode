using System.Threading.Tasks;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Modal.Service;

public interface IModalNavigationService
{
	Task OpenModalAsync(ModalParameter parameter);
	Task CloseModalAsync();
}