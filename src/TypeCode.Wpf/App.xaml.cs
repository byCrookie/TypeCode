using System.Windows;
using Nito.AsyncEx;
using TypeCode.Wpf.Helper.Boot;

namespace TypeCode.Wpf
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AsyncContext.Run(Bootstrapper.BootAsync);
        }
    }
}