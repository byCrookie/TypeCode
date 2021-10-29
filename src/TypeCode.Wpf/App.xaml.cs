using System.Windows;
using Autofac;
using TypeCode.Wpf.Helper.Navigation;

namespace TypeCode.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            BootStrap();
        }

        private static void BootStrap()
        {
            var builder = new ContainerBuilder();

            var mainView = new MainWindow();
            var frame = mainView.FindName("NavigationFrame");

            builder.RegisterInstance(frame).AsSelf().SingleInstance();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterModule<TypeCodeModule>();

            var container = builder.Build();

            var mainViewModel = container.Resolve<MainViewModel>();
            mainView.DataContext = mainViewModel;
            mainView.Show();
        }
    }
}