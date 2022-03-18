﻿using Autofac;
using Framework.Autofac.Boot;
using Framework.Autofac.Boot.Autofac;
using Framework.Autofac.Boot.Autofac.Registration;
using Framework.Autofac.Boot.Logger;
using Framework.Autofac.Boot.Start;
using Serilog;
using Serilog.Events;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Logging;
using TypeCode.Business.Modules;
using TypeCode.Console.Interactive.Modules;

namespace TypeCode.Console.Interactive.Boot;

public static class Bootstrapper
{
    public static Task BootAsync()
    {
        var bootScope = BootConfiguration.Configure<BootContext>(new List<Module>
        {
            new BootstrappingModule()
        });

        var bootFlow = bootScope.WorkflowBuilder
            .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                options => AutofacLoggerConfigurationProvider.Create(options).WriteTo.Console(LogEventLevel.Information)
            )
            .ThenAsync<IAutofacBootStep<BootContext, AutofacBootStepOptions>, AutofacBootStepOptions>(
                options => options.Autofac
                    .AddModule(new TypeCodeConsoleModule())
                    .AddModule(new TypeCodeBusinessModule())
            )
            .ThenAsync<IConfigurationAutofacLoadBootStep<BootContext>>()
            .ThenAsync<IStartBootStep<BootContext>>()
            .Build();

        return bootFlow.RunAsync(new BootContext(bootScope));
    }
}