using System.Threading.Tasks;
using Framework.Jab.Boot;
using Framework.Jab.Boot.Jab;
using Framework.Jab.Boot.Logger;
using Framework.Jab.Boot.Start;
using Serilog;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Logging;

namespace TypeCode.Wpf.Application.Boot;

public static class Bootstrapper
{
    public static Task BootAsync()
    {
        var bootScope = BootConfiguration.Configure<BootContext>(new TypeCodeWpfServiceProvider());

        var bootFlow = bootScope.WorkflowBuilder
            .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                options => LoggerConfigurationProvider.Create(options).WriteTo.Console()
            )
            .ThenAsync<IAssemblyLoadBootStep<BootContext>>()
            .ThenAsync<IStartBootStep<BootContext>>()
            .Build();

        return bootFlow.RunAsync(new BootContext(new TypeCodeWpfServiceProvider()));
    }
}