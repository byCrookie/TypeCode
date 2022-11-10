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
        var serviceProvider = await Bootstrapper.BootAsync(typeCodeCommandSettings.TargetDlls ?? Array.Empty<string>()).ConfigureAwait(false);
        await RunAsync(serviceProvider, context, settings).ConfigureAwait(false);
        return 0;
    }

    protected abstract Task RunAsync(TypeCodeConsoleServiceProvider serviceProvider, CommandContext context, TSettings settings);
}