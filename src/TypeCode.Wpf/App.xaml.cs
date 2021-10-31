using System;
using System.Windows;
using System.Windows.Threading;
using Nito.AsyncEx;
using TypeCode.Wpf.Application.Boot;

namespace TypeCode.Wpf
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            Current.DispatcherUnhandledException += HandleDispatcherUnhandledException;

            AsyncContext.Run(Bootstrapper.BootAsync);
        }

        private static void HandleDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.Exception?.Message ?? "Unhandled error occured. The application will be exited.");
        }
    }
}