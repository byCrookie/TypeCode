using System;
using System.Windows.Controls;
using System.Windows.Threading;
using Autofac;
using Framework.Autofac.Factory;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Helper.Navigation
{
	public class NavigationService : INavigationService
	{
		private readonly Frame _frame;
		private readonly IFactory _factory;

		public NavigationService(Frame frame, IFactory factory)
		{
			_frame = frame;
			_factory = factory;
		}

		public void Navigate<T>(object parameter)
		{
			var viewModelType = typeof(T);
			var viewModelInstance = _factory.Create<T>();

			var viewName = viewModelType.Name[..^"Model".Length];
			var viewType = Type.GetType($"{viewModelType.Namespace}.{viewName}");

			if (viewType is null || Activator.CreateInstance(viewType) is not UserControl viewInstance)
			{
				throw new ApplicationException($"View zu {viewModelType.Name} nicht gefunden");
			}

			viewInstance.DataContext = viewModelInstance;

			_frame.Navigate(viewInstance);

			var viewModelBase = viewModelInstance as ViewModelBase;
			viewModelBase?.OnNavigateTo(parameter);
		}
	}
}