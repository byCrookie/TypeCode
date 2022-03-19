using Framework.Jab.Boot.Logger;
using Serilog;

namespace TypeCode.Business.Logging;

public static class JabLoggerConfigurationProvider
{
    private const string LogFile = "TypeCode.log.txt";

    public static LoggerConfiguration Create()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.File(LogFile, shared: true);
    }

    public static LoggerConfiguration Create(LoggerBootStepOptions loggerBootStepOptions)
    {
        return loggerBootStepOptions
            .Configuration
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.File(LogFile, shared: true);
    }
}