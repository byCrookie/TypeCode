using Framework.Boot.Logger;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using TypeCode.Business.Bootstrapping.Data;

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
            .WriteTo.File(Path.Combine("logs", LogFileNames.File), rollOnFileSizeLimit: true, shared: true);
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
            .WriteTo.File(Path.Combine("logs", LogFileNames.File), rollOnFileSizeLimit: true, shared: true);
    }

    public static LoggerConfiguration Create(LoggerBootStepOptions loggerBootStepOptions, IUserDataLocationProvider userDataLocationProvider)
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
            .WriteTo.File(Path.Combine(userDataLocationProvider.GetLogsPath(), LogFileNames.File), rollOnFileSizeLimit: true, shared: true);
    }
}