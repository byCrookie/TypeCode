using System.Windows;
using Nito.AsyncEx;
using TypeCode.Wpf.Helper.Boot;

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

            Dispatcher.Invoke(() => AsyncContext.Run(Bootstrapper.BootAsync));
        }
    }
}