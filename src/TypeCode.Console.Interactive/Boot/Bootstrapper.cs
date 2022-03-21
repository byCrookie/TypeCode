﻿using Framework.Jab.Boot;
using Framework.Jab.Boot.Jab;
using Framework.Jab.Boot.Logger;
using Framework.Jab.Boot.Start;
using Serilog;
using Serilog.Events;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Logging;

namespace TypeCode.Console.Interactive.Boot;

public static class Bootstrapper
{
    public static Task BootAsync()
    {
        var bootScope = BootConfiguration.Configure<BootContext>(new TypeCodeConsoleInteractiveServiceProvider());

        var bootFlow = bootScope.WorkflowBuilder
            .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                options => LoggerConfigurationProvider.Create(options)
                    .WriteTo.Console(LogEventLevel.Information)
            )
            .ThenAsync<IConfigurationLoadBootStep<BootContext>>()
            .ThenAsync<IStartBootStep<BootContext>>()
            .Build();

        return bootFlow.RunAsync(new BootContext(bootScope));
    }
}