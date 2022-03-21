using System.Reflection;
using Framework.Jab.Boot;
using Framework.Jab.Boot.Jab;
using Framework.Jab.Boot.Logger;
using Serilog;
using Serilog.Events;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Logging;

namespace TypeCode.Console.Boot;

public static class Bootstrapper
{
    public static Task<BootContext> BootAsync()
    {
        var bootScope = BootConfiguration.Configure<BootContext>(new TypeCodeConsoleServiceProvider());

        var bootFlow = bootScope.WorkflowBuilder
            .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                options => LoggerConfigurationProvider.Create(options).WriteTo.Console(LogEventLevel.Information)
            )
            .ThenAsync<IConfigurationLoadBootStep<BootContext>>()
            .Build();

        return bootFlow.RunAsync(new BootContext(bootScope));
    }
}