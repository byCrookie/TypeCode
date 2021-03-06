using System.IO;
using System.Windows;
using System.Windows.Threading;
using Framework.Boot.Logger;
using Framework.Extensions.List;
using Nito.AsyncEx;
using Serilog;
using Serilog.Events;
using TypeCode.Business.Logging;
using TypeCode.Wpf.Application.Boot;

namespace TypeCode.Wpf;

public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            var options = new LoggerBootStepOptions();
            options.Configuration.WriteTo.Console(LogEventLevel.Debug);
            
            LogFilePaths.AllPaths.ForEach(path =>
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            });
        
            Log.Logger = LoggerConfigurationProvider.Create(options).CreateLogger();
            
            Current.DispatcherUnhandledException += HandleDispatcherUnhandledException;

            AsyncContext.Run(Bootstrapper.BootAsync);
            
            base.OnStartup(e);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            File.AppendAllText(LogFilePaths.FileFatal, $"{exception.Message} - {exception.InnerException?.Message} | {exception.StackTrace}");
            throw;
        }
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