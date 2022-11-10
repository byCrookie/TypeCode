using Serilog;
using Spectre.Console.Cli;
using TypeCode.Business.Logging;
using TypeCode.Console.Boot;
using TypeCode.Console.Boot.Helper;
using TypeCode.Console.Commands.Builder;
using TypeCode.Console.Commands.Composer;
using TypeCode.Console.Commands.Guid;
using TypeCode.Console.Commands.Mapper;
using TypeCode.Console.Commands.Specflow;
using TypeCode.Console.Commands.Unit;

namespace TypeCode.Console;

public sealed class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = LoggerConfigurationProvider.Create().CreateLogger();

        try
        {
            Log.Debug("Boot");
            await Bootstrapper.BootAsync().ConfigureAwait(false);
            
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<UnitManuallyCommand>("unit-manually");
                config.AddCommand<UnitTypeCommand>("unit-type");
                config.AddCommand<SpecflowCommand>("specflow");
                config.AddCommand<BuilderCommand>("builder");
                config.AddCommand<ComposerCommand>("composer");
                config.AddCommand<MapperCommand>("mapper");
                config.AddCommand<GuidCommand>("guid");
            });
            await app.RunAsync(args).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            ExceptionLogger.WriteExceptionToLog(exception);
            return 1;
        }

        return 0;
    }
}