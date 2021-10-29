using System;
using System.Windows.Controls;
using Autofac;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Helper.Navigation
{
	public class NavigationService : INavigationService
	{
		private readonly Frame _frame;
		private readonly IComponentContext _componentContext;

		public NavigationService(Frame frame, IComponentContext componentContext)
		{
			_frame = frame;
			_componentContext = componentContext;
		}

		public void Navigate<T>(object parameter)
		{
			var viewModelType = typeof(T);
			var viewModelInstance = _componentContext.Resolve<T>();

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