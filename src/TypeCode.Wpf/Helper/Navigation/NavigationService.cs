using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Framework.Autofac.Factory;
using TypeCode.Wpf.Helper.Modal;

namespace TypeCode.Wpf.Helper.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly MainWindow _mainWindow;
        private readonly IFactory _factory;
        private object _lastViewModel;
        private ModalParameter _lastModalParameter;

        public NavigationService(MainWindow mainWindow, IFactory factory)
        {
            _mainWindow = mainWindow;
            _factory = factory;
        }

        public async Task OpenModal(ModalParameter modalParameter)
        {
            var viewModelType = typeof(ModalViewModel);
            var viewModelInstance = _factory.Create<ModalViewModel>();

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

            _mainWindow.MainContent.Opacity = 0.1;
            _mainWindow.MainContent.IsEnabled = false;
            _mainWindow.ModalContent.Visibility = Visibility.Visible;
            if (!_mainWindow.ModalFrame.Navigate(viewInstance))
            {
                throw new ApplicationException($"Navigation to modal {viewModelType.Name} failed");
            }

            var context = new NavigationContext();
            context.AddParameter(modalParameter);
            await CallOnNavigatedToOnCurrentViewModelAsync(context, viewModelInstance).ConfigureAwait(true);

            SetLastModalParameterToCurrent(modalParameter);
        }

        public Task CloseModal()
        {
            _mainWindow.MainContent.Opacity = 1;
            _mainWindow.MainContent.IsEnabled = true;
            _mainWindow.ModalContent.Visibility = Visibility.Collapsed;

            return _lastModalParameter.OnCloseAsync?.Invoke() ?? Task.CompletedTask;
        }

        public async Task NavigateAsync<T>(NavigationContext context = null)
        {
            await CallNavigatedFromOnLastViewModelAsync(context).ConfigureAwait(true);

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

            await CallOnNavigatedToOnCurrentViewModelAsync(context, viewModelInstance).ConfigureAwait(true);
            
            SetCurrentViewModelToLastViewModel(viewModelInstance);
        }
        
        private void SetLastModalParameterToCurrent(ModalParameter parameter)
        {
            _lastModalParameter = parameter;
        }

        private void SetCurrentViewModelToLastViewModel<T>(T viewModelInstance)
        {
            _lastViewModel = viewModelInstance;
        }

        private static Task CallOnNavigatedToOnCurrentViewModelAsync<T>(NavigationContext context, T viewModelInstance)
        {
            if (viewModelInstance is IAsyncNavigatedTo asyncNavigatedTo)
            {
                return asyncNavigatedTo.OnNavigatedToAsync(context ?? new NavigationContext());
            }
            
            return Task.CompletedTask;
        }

        private Task CallNavigatedFromOnLastViewModelAsync(NavigationContext context)
        {
            return _lastViewModel is IAsyncNavigatedFrom asyncNavigatedFrom
                ? asyncNavigatedFrom.OnNavigatedFromAsync(context ?? new NavigationContext())
                : Task.CompletedTask;
        }
    }
}