using System.Threading.Tasks;
using TypeCode.Wpf.Helper.Modal;

namespace TypeCode.Wpf.Helper.Navigation
{
	public interface INavigationService
	{
		Task NavigateAsync<T>(NavigationContext context = null);
		Task OpenModal(ModalParameter parameter);
		Task CloseModal();
	}
}
