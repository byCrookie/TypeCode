using Serilog;
using Spectre.Console.Cli;
using TypeCode.Console.Boot;

namespace TypeCode.Console.Commands;

internal abstract class TypeCodeCommand<TSettings> : AsyncCommand<TSettings> where TSettings : TypeCodeCommandSettings
{
    public override async Task<int> ExecuteAsync(CommandContext context, TSettings settings)
    {
        Log.Debug("Boot");
        var typeCodeCommandSettings = settings as TypeCodeCommandSettings;
        var options = new TargetDllsBootStepOptions
        {
            DllPaths = typeCodeCommandSettings.DllPaths is null ? new List<string>() : typeCodeCommandSettings.DllPaths,
            DllDeep = typeCodeCommandSettings.DllDeep,
            DllPattern = typeCodeCommandSettings.DllPattern
        };
        var serviceProvider = await Bootstrapper.BootAsync(options).ConfigureAwait(false);
        await RunAsync(serviceProvider, context, settings).ConfigureAwait(false);
        return 0;
    }

    protected abstract Task RunAsync(TypeCodeConsoleServiceProvider serviceProvider, CommandContext context, TSettings settings);
}