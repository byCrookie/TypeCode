using Autofac;
using Framework.Autofac.Boot;
using Framework.Autofac.Boot.Autofac;
using Framework.Autofac.Boot.Autofac.Registration;
using Framework.Autofac.Boot.Logger;
using Serilog;
using Serilog.Events;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Logging;
using TypeCode.Business.Modules;

namespace TypeCode.Console.Boot;

public static class Bootstrapper
{
    public static Task<BootContext> BootAsync()
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
                    .AddModule(new TypeCodeBusinessModule())
            )
            .ThenAsync<IConfigurationLoadBootStep<BootContext>>()
            .Build();

        return bootFlow.RunAsync(new BootContext(bootScope));
    }
}