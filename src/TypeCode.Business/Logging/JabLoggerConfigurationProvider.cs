using Framework.Jab.Boot.Logger;
using Serilog;

namespace TypeCode.Business.Logging;

public static class JabLoggerConfigurationProvider
{
    public static LoggerConfiguration Create()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.File(LogFiles.File, rollOnFileSizeLimit: true, shared: true);
    }

    public static LoggerConfiguration Create(LoggerBootStepOptions loggerBootStepOptions)
    {
        return loggerBootStepOptions
            .Configuration
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.File(LogFiles.File, rollOnFileSizeLimit: true, shared: true);
    }
}