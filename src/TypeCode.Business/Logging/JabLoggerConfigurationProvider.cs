﻿using Framework.Jab.Boot.Logger;
using Serilog;
using Serilog.Events;

namespace TypeCode.Business.Logging;

public static class JabLoggerConfigurationProvider
{
    private const string LogFile = "TypeCode.log.txt";
        
    public static LoggerConfiguration Create()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug(LogEventLevel.Debug)
            .WriteTo.Console(LogEventLevel.Debug)
            .WriteTo.File(LogFile, LogEventLevel.Debug);
    }

    public static LoggerConfiguration Create(LoggerBootStepOptions loggerBootStepOptions)
    {
        return loggerBootStepOptions
            .Configuration
            .MinimumLevel.Debug()
            .WriteTo.Debug(LogEventLevel.Debug)
            .WriteTo.Console(LogEventLevel.Debug)
            .WriteTo.File(LogFile, LogEventLevel.Debug);
    }
}