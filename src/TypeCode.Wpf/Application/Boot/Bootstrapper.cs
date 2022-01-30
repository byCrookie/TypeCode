using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Framework.Autofac.Boot;
using Framework.Autofac.Boot.Autofac;
using Framework.Autofac.Boot.Autofac.Registration;
using Framework.Autofac.Boot.Logger;
using Framework.Autofac.Boot.Start;
using Serilog;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Logging;
using TypeCode.Business.Modules;
using TypeCode.Wpf.Application.Boot.SetupWpfApplication;

namespace TypeCode.Wpf.Application.Boot;

public static class Bootstrapper
{
    public static Task BootAsync()
    {
        var bootScope = BootConfiguration.Configure<BootContext>(new List<Module>
        {
            new BootModule()
        });

        var bootFlow = bootScope.WorkflowBuilder
            .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                options => LoggerConfigurationProvider.Create(options).WriteTo.Console()
            )
            .ThenAsync<ISetupWpfApplicationStep<BootContext>>()
            .ThenAsync<IAutofacBootStep<BootContext, AutofacBootStepOptions>, AutofacBootStepOptions>(
                options => options.Autofac
                    .AddModule(new TypeCodeWpfModule())
                    .AddModule(new TypeCodeBusinessModule())
            )
            .ThenAsync<IAssemblyLoadBootStep<BootContext>>()
            .ThenAsync<IStartBootStep<BootContext>>()
            .Build();

        return bootFlow.RunAsync(new BootContext(bootScope));
    }
}