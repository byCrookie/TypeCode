using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Unit;

public class UnitTypeCommand : AsyncCommand<UnitTypeCommand.Settings>
{
    public class Settings : CommandSettings
    {
        public Settings(string[] typeNames)
        {
            TypeNames = typeNames;
        }
        
        [Description("Typenames for which the tables are generated.")]
        [CommandArgument(0, "[TypeNames]")]
        public string[] TypeNames { get; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var mode = serviceProvider.GetService<ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter>>();
        var typeProvider = serviceProvider.GetService<ITypeProvider>();
        var types = typeProvider.TryGetByNames(settings.TypeNames);
        var parameter = new UnitTestDependencyTypeGeneratorParameter { Types = types.ToList() };
        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        return 0;
    }
}