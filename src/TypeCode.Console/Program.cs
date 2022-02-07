using System.Reflection;
using Serilog;
using TypeCode.Business.Logging;
using TypeCode.Console.Boot;

Log.Logger = LoggerConfigurationProvider.Create().CreateLogger();

try
{
    Log.Debug("Boot");
    await Bootstrapper.BootAsync().ConfigureAwait(false);
}
catch (Exception exception)
{
    WriteExceptionToLog(exception);
    return 1;
}

return 0;

static void WriteExceptionToLog(Exception exception)
{
    Log.Error(exception, "{0}", exception.Message);

    if (exception is ReflectionTypeLoadException reflectionTypeLoadException)
    {
        foreach (var loaderException in reflectionTypeLoadException.LoaderExceptions)
        {
            Log.Error(loaderException ?? new Exception("Type Load Exception"), "{0}", loaderException?.Message ?? "Type Load Exception");
        }
    }
}