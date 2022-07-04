using Framework.Boot;
using Framework.Boot.Configuration;
using Framework.Boot.Logger;
using Framework.Boot.Start;
using TypeCode.Business.Logging;
using TypeCode.Wpf.Application.Boot.SetupWpfApplication;

namespace TypeCode.Wpf.Application.Boot;

public static class Bootstrapper
{
    public static Task BootAsync()
    {
        var serviceProvider = new TypeCodeWpfServiceProvider();
        var bootScope = BootConfiguration.Configure<BootContext>(serviceProvider);

        var bootFlow = bootScope.WorkflowBuilder
            .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                options => LoggerConfigurationProvider.Create(options)
            )
            .ThenAsync<ISetupWpfApplicationStep<BootContext>>()
            .ThenAsync<IStartBootStep<BootContext>>()
            .Build();

        return bootFlow.RunAsync(new BootContext(bootScope));
    }
}