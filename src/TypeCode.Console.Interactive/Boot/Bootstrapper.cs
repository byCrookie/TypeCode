using Framework.Boot;
using Framework.Boot.Configuration;
using Framework.Boot.Logger;
using Framework.Boot.Start;
using TypeCode.Business.Bootstrapping.Configuration;
using TypeCode.Business.Bootstrapping.Data;
using TypeCode.Business.Logging;

namespace TypeCode.Console.Interactive.Boot;

public static class Bootstrapper
{
    public static Task BootAsync()
    {
        var bootScope = BootConfiguration.Configure<BootContext>(new TypeCodeConsoleInteractiveServiceProvider());

        var bootFlow = bootScope.WorkflowBuilder
            .ThenAsync<IUserDataInitializeBootStep<BootContext>>()
            .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                options => LoggerConfigurationProvider.Create(options)
            )
            .ThenAsync<IConfigurationLoadBootStep<BootContext>>()
            .ThenAsync<IStartBootStep<BootContext>>()
            .Build();

        return bootFlow.RunAsync(new BootContext(bootScope));
    }
}