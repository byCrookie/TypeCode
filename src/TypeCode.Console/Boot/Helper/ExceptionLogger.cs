using System.Reflection;
using Serilog;

namespace TypeCode.Console.Boot.Helper;

public static class ExceptionLogger
{
    public static void WriteExceptionToLog(Exception exception)
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
}