using Autofac;
using Cocona;
using Serilog;
using TypeCode.Business.Configuration;
using TypeCode.Business.Logging;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Boot;
using TypeCode.Console.Boot.Helper;
using TypeCode.Console.Commands.Builder;
using TypeCode.Console.Commands.Composer;
using TypeCode.Console.Commands.Mapper;
using TypeCode.Console.Commands.Specflow;
using TypeCode.Console.Commands.Unit;

namespace TypeCode.Console;

[HasSubCommands(typeof(UnitManuallyCommand), "unit-manually")]
[HasSubCommands(typeof(UnitTypeCommand), "unit-type")]
[HasSubCommands(typeof(SpecflowCommand), "specflow")]
[HasSubCommands(typeof(BuilderCommand), "builder")]
[HasSubCommands(typeof(ComposerCommand), "composer")]
[HasSubCommands(typeof(MapperCommand), "mapper")]
public class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = AutofacLoggerConfigurationProvider.Create().CreateLogger();

        try
        {
            Log.Debug("Boot");
            var context = await Bootstrapper.BootAsync().ConfigureAwait(false);
            ContextProvider.Set(context);
            
            await using (var scope = LifeTimeScopeCreator.BeginLifetimeScope(ContextProvider.Get()))
            {
                var configurationProvider = scope.Resolve<IConfigurationProvider>();
                var typeProvider = scope.Resolve<ITypeProvider>();
                typeProvider.Initalize(configurationProvider.GetConfiguration());
            }

            await CoconaApp.RunAsync<Program>(args).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            ExceptionLogger.WriteExceptionToLog(exception);
            return 1;
        }

        return 0;
    }
}