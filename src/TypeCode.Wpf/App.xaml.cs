using System.Windows;
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
            Bootstrapper.Boot();
        }
    }
}