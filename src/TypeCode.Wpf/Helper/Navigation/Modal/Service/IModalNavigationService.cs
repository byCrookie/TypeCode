namespace TypeCode.Wpf.Helper.Navigation.Modal.Service;

public interface IModalNavigationService
{
	Task OpenModalAsync(ModalParameter parameter);
	Task CloseModalAsync();
}