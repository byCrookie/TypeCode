using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Framework.Autofac.Factory;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Helper.Navigation.Modal.View;
using TypeCode.Wpf.Main;
using TypeCode.Wpf.Main.Content;

namespace TypeCode.Wpf.Helper.Navigation.Service
{
    public class NavigationService : INavigationService
    {
        private readonly MainWindow _mainWindow;
        private readonly MainContentView _mainContentView;
        private readonly IFactory _factory;
        private object _lastViewModel;
        private ModalParameter _lastModalParameter;

        public NavigationService(MainWindow mainWindow, MainContentView mainContentView, IFactory factory)
        {
            _mainWindow = mainWindow;
            _mainContentView = mainContentView;
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

            _mainWindow.Main.Opacity = 0.1;
            _mainWindow.Main.IsEnabled = false;
            _mainWindow.ModalOverlay.Visibility = Visibility.Visible;
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
            _mainWindow.Main.Opacity = 1;
            _mainWindow.Main.IsEnabled = true;
            _mainWindow.ModalOverlay.Visibility = Visibility.Collapsed;

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

            if (!_mainContentView.NavigationFrame.Navigate(viewInstance))
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