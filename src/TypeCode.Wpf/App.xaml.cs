using System;
using System.Windows;
using System.Windows.Threading;
using Nito.AsyncEx;
using TypeCode.Wpf.Helper.Boot;
using TypeCode.Wpf.Helper.MessageBoxes;

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
            Handle(e.Exception);
            e.Handled = true;
        }

        private static void Handle(Exception exception)
        {
            InteractionBox.Show(exception?.Message ?? "Unhandled error occured.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}