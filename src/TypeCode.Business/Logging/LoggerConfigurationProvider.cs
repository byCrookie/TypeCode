using Framework.Jab.Boot.Logger;
using Serilog;
using Serilog.Events;

namespace TypeCode.Business.Logging;

public static class LoggerConfigurationProvider
{
    public static LoggerConfiguration Create()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.Console(LogEventLevel.Information)
            .WriteTo.File(LogFiles.File, rollOnFileSizeLimit: true, shared: true);
    }

    public static LoggerConfiguration Create(LoggerBootStepOptions loggerBootStepOptions)
    {
        return loggerBootStepOptions
            .Configuration
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.Console(LogEventLevel.Information)
            .WriteTo.File(LogFiles.File, rollOnFileSizeLimit: true, shared: true);
    }
}