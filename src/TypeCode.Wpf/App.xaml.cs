using System.Windows;
using System.Windows.Threading;
using Nito.AsyncEx;
using Serilog;
using TypeCode.Wpf.Application.Boot;

namespace TypeCode.Wpf
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();
            
            Current.DispatcherUnhandledException += HandleDispatcherUnhandledException;

            AsyncContext.Run(Bootstrapper.BootAsync);
            
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Debug("Application exited with {0}", e.ApplicationExitCode);
            base.OnExit(e);
        }

        private static void HandleDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Fatal(e.Exception, "Unhandled error occured - Exit application");
        }
    }
}