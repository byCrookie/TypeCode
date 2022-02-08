using Autofac;
using Cocona;
using Serilog;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Logging;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Boot;
using TypeCode.Console.Boot.Helper;
using TypeCode.Console.Commands.Unit;

namespace TypeCode.Console;

[HasSubCommands(typeof(UnitManuallyCommand), "unit-manually")]
[HasSubCommands(typeof(UnitTypeCommand), "unit-type")]
public class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = LoggerConfigurationProvider.Create().CreateLogger();

        try
        {
            Log.Debug("Boot");
            var context = await Bootstrapper.BootAsync().ConfigureAwait(false);
            ContextProvider.Set(context);
            
            await using (var scope = LifeTimeScopeCreator.BeginLifetimeScope(ContextProvider.Get()))
            {
                var typeEvaluator = scope.Resolve<ITypeEvaluator>();
                var configuration = typeEvaluator.EvaluateTypes(AssemblyLoadProvider.GetConfiguration());
                var typeProvider = scope.Resolve<ITypeProvider>();
                typeProvider.Initalize(configuration);
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