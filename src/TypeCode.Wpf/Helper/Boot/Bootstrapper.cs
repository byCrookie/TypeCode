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
using TypeCode.Wpf.Helper.Boot.SetupWpfApplication;

namespace TypeCode.Wpf.Helper.Boot
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
                // .ThenAsync<IDisposeBootLifeTimeScopeStep<BootContext>>()
                .ThenAsync<IFrameworkConfigurationBootStep<BootContext, FrameworkBootStepOptions>,
                    FrameworkBootStepOptions>(
                    config => { config.ConfigurationFile = "TypeCode.Wpf.cfg.xml"; }
                )
                .ThenAsync<IAssemblyBootStep<BootContext>>()
                .ThenAsync<ITypeBootStep<BootContext>>()
                .ThenAsync<IModuleCatalogBootStep<BootContext>>()
                .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                    config => { config.Log4NetConfigurationFile = "TypeCode.Wpf.cfg.xml"; }
                )
                .ThenAsync<ISetupWpfApplicationStep<BootContext>>()
                // .ThenAsync<IBeginLifeTimeScopeBootStep<BootContext>>()
                .ThenAsync<IAssemblyLoadBootStep<BootContext>>()
                .ThenAsync<IStartBootStep<BootContext>>()
                .Build();

            await bootFlow.RunAsync(new BootContext(bootScope.Container, bootScope.LifeTimeScope)).ConfigureAwait(false);
        }
    }
}