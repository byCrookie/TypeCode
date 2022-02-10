using Framework.Autofac.Boot.Logger;
using Serilog;
using Serilog.Events;

namespace TypeCode.Business.Logging;

public static class LoggerConfigurationProvider
{
    private const string LogFile = "TypeCode.log.txt";
        
    public static LoggerConfiguration Create()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(LogFile, LogEventLevel.Debug);
    }

    public static LoggerConfiguration Create(LoggerBootStepOptions loggerBootStepOptions)
    {
        return loggerBootStepOptions
            .Configuration
            .MinimumLevel.Debug()
            .WriteTo.File(LogFile, LogEventLevel.Debug);
    }
}