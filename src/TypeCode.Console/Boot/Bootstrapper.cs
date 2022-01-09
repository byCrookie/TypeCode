using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Framework.Boot;
using Framework.Boot.Autofac;
using Framework.Boot.Autofac.Registration;
using Framework.Boot.Logger;
using Framework.Boot.Start;
using Serilog;
using Serilog.Events;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Logging;
using TypeCode.Business.Modules;
using TypeCode.Console.Modules;

namespace TypeCode.Console.Boot
{
    public static class Bootstrapper
    {
        public static async Task BootAsync()
        {
            var bootScope = BootConfiguration.Configure<BootContext>(new List<Module>
            {
                new BootstrappingModule()
            });

            var bootFlow = bootScope.WorkflowBuilder
                .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                    options => LoggerConfigurationProvider.Create(options).WriteTo.Console(LogEventLevel.Information)
                )
                .ThenAsync<IAutofacBootStep<BootContext, AutofacBootStepOptions>, AutofacBootStepOptions>(
                    options => options.Autofac
                        .AddModuleCatalog(new TypeCodeConsoleModuleCatalog())
                        .AddModuleCatalog(new TypeCodeBusinessModuleCatalog())
                )
                .ThenAsync<IAssemblyLoadBootStep<BootContext>>()
                .ThenAsync<IStartBootStep<BootContext>>()
                .Build();

            await bootFlow.RunAsync(new BootContext(bootScope.Container, bootScope.LifeTimeScope)).ConfigureAwait(false);
        }
    }
}