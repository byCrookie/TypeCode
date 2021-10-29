using System;
using Autofac;

namespace TypeCode.Wpf.Helper.Boot
{
    public static class Bootstrapper
    {
        public static void Boot()
        {
            var builder = new ContainerBuilder();

            var mainView = new MainWindow();
            var frame = mainView.FindName("NavigationFrame");

            if (frame is null)
            {
                throw new ApplicationException("MainView doesn't implement navigation frame");
            }

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