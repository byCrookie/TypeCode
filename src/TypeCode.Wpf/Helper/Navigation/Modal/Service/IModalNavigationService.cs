using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.Navigation.Modal.Service
{
	public interface IModalNavigationService
	{
		Task OpenModal(ModalParameter parameter);
		Task CloseModal();
	}
}
