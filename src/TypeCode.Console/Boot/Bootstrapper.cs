using Framework.Boot;
using Framework.Boot.Configuration;
using Framework.Boot.Logger;
using TypeCode.Business.Bootstrapping.Configuration;
using TypeCode.Business.Bootstrapping.Data;
using TypeCode.Business.Logging;

namespace TypeCode.Console.Boot;

public static class Bootstrapper
{
    public static Task<BootContext> BootAsync()
    {
        var bootScope = BootConfiguration.Configure<BootContext>(new TypeCodeConsoleServiceProvider());

        var bootFlow = bootScope.WorkflowBuilder
            .ThenAsync<IUserDataInitializeBootStep<BootContext>>()
            .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                options => LoggerConfigurationProvider.Create(options)
            )
            .ThenAsync<IConfigurationLoadBootStep<BootContext>>()
            .Build();

        return bootFlow.RunAsync(new BootContext(bootScope));
    }
}