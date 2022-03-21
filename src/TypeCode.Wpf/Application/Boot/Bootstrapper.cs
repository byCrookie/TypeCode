using Framework.Jab.Boot;
using Framework.Jab.Boot.Jab;
using Framework.Jab.Boot.Logger;
using Framework.Jab.Boot.Start;
using TypeCode.Business.Bootstrapping;
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
                options => JabLoggerConfigurationProvider.Create(options)
            )
            .ThenAsync<ISetupWpfApplicationStep<BootContext>>()
            .ThenAsync<IStartBootStep<BootContext>>()
            .Build();

        return bootFlow.RunAsync(new BootContext(bootScope));
    }
}