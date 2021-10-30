using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Autofac;
using Framework.Autofac.Factory;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Helper.Navigation
{
	public class NavigationService : INavigationService
	{
		private readonly MainWindow _mainWindow;
		private readonly IFactory _factory;

		public NavigationService(MainWindow mainWindow, IFactory factory)
		{
			_mainWindow = mainWindow;
			_factory = factory;
		}

		public async Task NavigateAsync<T>(object parameter) where T : ViewModelBase
		{
			var viewModelType = typeof(T);
			var viewModelInstance = _factory.Create<T>();
			
			if (viewModelInstance is null)
			{
				throw new ApplicationException($"ViewModel of {viewModelType.Name} not found");
			}

			var viewName = viewModelType.Name[..^"Model".Length];
			var viewType = Type.GetType($"{viewModelType.Namespace}.{viewName}");

			if (viewType is null || Activator.CreateInstance(viewType) is not UserControl viewInstance)
			{
				throw new ApplicationException($"View of {viewModelType.Name} not found");
			}

			viewInstance.DataContext = viewModelInstance;

			if (!_mainWindow.NavigationFrame.Navigate(viewInstance))
			{
				throw new ApplicationException($"Navigation to {viewModelType.Name} failed");
			}

			await viewModelInstance.OnNavigateToAsync(parameter).ConfigureAwait(true);
		}
	}
}