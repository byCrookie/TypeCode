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
            Current.DispatcherUnhandledException += HandleDispatcherUnhandledException;

            AsyncContext.Run(Bootstrapper.BootAsync);
            
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Console.WriteLine($@"Application exited with {e.ApplicationExitCode}");
            base.OnExit(e);
        }

        private static void HandleDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.Exception?.Message ?? "Unhandled error occured. The application will be exited.");
        }
    }
}