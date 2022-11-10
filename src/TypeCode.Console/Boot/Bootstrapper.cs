using Framework.Boot;
using Framework.Boot.Configuration;
using Framework.Boot.Logger;
using TypeCode.Business.Bootstrapping.Configuration;
using TypeCode.Business.Bootstrapping.Data;
using TypeCode.Business.Logging;

namespace TypeCode.Console.Boot;

public static class Bootstrapper
{
    internal static async Task<TypeCodeConsoleServiceProvider> BootAsync(TargetDllsBootStepOptions? dllsBootStepOptions)
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var bootScope = BootConfiguration.Configure<BootContext>(serviceProvider);

        var bootFlow = bootScope.WorkflowBuilder
            .ThenAsync<IUserDataInitializeBootStep<BootContext>>()
            .ThenAsync<ILoggerBootStep<BootContext, LoggerBootStepOptions>, LoggerBootStepOptions>(
                options => LoggerConfigurationProvider.Create(options)
            )
            .IfElseFlow(_ => dllsBootStepOptions is not null,
                ifFlow => ifFlow.ThenAsync<ITargetDllsLoadBootStep<BootContext, TargetDllsBootStepOptions>, TargetDllsBootStepOptions>(options =>
                {
                    options.DllPaths = dllsBootStepOptions!.DllPaths;
                    options.DllDeep = dllsBootStepOptions.DllDeep;
                    options.DllPattern = dllsBootStepOptions.DllPattern;
                }),
                elseFlow => elseFlow.ThenAsync<IConfigurationLoadBootStep<BootContext>>()
            )
            .Build();

        await bootFlow.RunAsync(new BootContext(bootScope)).ConfigureAwait(false);
        return serviceProvider;
    }
}