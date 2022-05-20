using Framework.Jab.Boot.Logger;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace TypeCode.Business.Logging;

public static class LoggerConfigurationProvider
{
    public static LoggerConfiguration Create()
    {
        return new LoggerConfiguration()
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.Console(LogEventLevel.Information)
            .WriteTo.File(LogFilePaths.File, rollOnFileSizeLimit: true, shared: true);
    }

    public static LoggerConfiguration Create(LoggerBootStepOptions loggerBootStepOptions)
    {
        return loggerBootStepOptions
            .Configuration
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.Console(LogEventLevel.Information)
            .WriteTo.File(LogFilePaths.File, rollOnFileSizeLimit: true, shared: true);
    }
}