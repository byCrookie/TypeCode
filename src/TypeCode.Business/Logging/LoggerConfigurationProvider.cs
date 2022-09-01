using Framework.Boot.Logger;
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
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Error()   
#endif
            .WriteTo.Debug(LogEventLevel.Debug)
            .WriteTo.Console(LogEventLevel.Information)
            .WriteTo.File(LogFilePaths.File, rollOnFileSizeLimit: true, shared: true);
    }

    public static LoggerConfiguration Create(LoggerBootStepOptions loggerBootStepOptions)
    {
        return loggerBootStepOptions
            .Configuration
            .Enrich.WithExceptionDetails()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Error()   
#endif
            .WriteTo.Debug(LogEventLevel.Debug)
            .WriteTo.Console(LogEventLevel.Information)
            .WriteTo.File(LogFilePaths.File, rollOnFileSizeLimit: true, shared: true);
    }
}