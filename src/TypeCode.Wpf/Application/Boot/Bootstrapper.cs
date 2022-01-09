using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Framework.Boot;
using Framework.Boot.Autofac;
using Framework.Boot.Autofac.Registration;
using Framework.Boot.Logger;
using Framework.Boot.Start;
using Serilog;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Logging;
using TypeCode.Business.Modules;
using TypeCode.Wpf.Application.Boot.SetupWpfApplication;

namespace TypeCode.Wpf.Application.Boot
{
    public static class Bootstrapper
    {
        public static async Task BootAsync()
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
                        .AddModuleCatalog(new TypeCodeWpfModuleCatalog())
                        .AddModuleCatalog(new TypeCodeBusinessModuleCatalog())
                )
                .ThenAsync<IAssemblyLoadBootStep<BootContext>>()
                .ThenAsync<IStartBootStep<BootContext>>()
                .Build();

            await bootFlow.RunAsync(new BootContext(bootScope.Container, bootScope.LifeTimeScope)).ConfigureAwait(false);
        }
    }
}