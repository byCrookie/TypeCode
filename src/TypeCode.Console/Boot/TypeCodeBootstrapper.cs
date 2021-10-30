using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Framework.Boot;
using Framework.Boot.AssemblyLoad;
using Framework.Boot.Autofac;
using Framework.Boot.Autofac.ModuleCatalog;
using Framework.Boot.Configuration;
using Framework.Boot.Logger;
using Framework.Boot.Start;
using Framework.Boot.TypeLoad;
using TypeCode.Business.Bootstrapping;

namespace TypeCode.Console.Boot
{
    public static class TypeCodeBootstrapper
    {
        public static async Task BootAsync()
        {
            var bootScope = BootConfiguration.Configure<BootContext>(new List<Module>
            {
                new BootstrappingModule()
            });

            var bootFlow = bootScope.WorkflowBuilder
                .ThenAsync<IFrameworkConfigurationBootStep<BootContext, FrameworkBootStepOptions>,
                    FrameworkBootStepOptions>(
                    config => { config.ConfigurationFile = "TypeCode.Console.cfg.xml"; }
                )
                .ThenAsync<IAssemblyBootStep<BootContext>>()
                .ThenAsync<ITypeBootStep<BootContext>>()
                .ThenAsync<IModuleCatalogBootStep<BootContext>>()
                .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                    config => { config.Log4NetConfigurationFile = "TypeCode.Console.cfg.xml"; }
                )
                .ThenAsync<IAssemblyLoadBootStep<BootContext>>()
                .ThenAsync<IStartBootStep<BootContext>>()
                .Build();

            await bootFlow.RunAsync(new BootContext(bootScope.Container, bootScope.LifeTimeScope)).ConfigureAwait(false);
        }
    }
}