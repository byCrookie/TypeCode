using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Unit;

internal sealed class UnitTypeCommand : TypeCodeCommand<UnitTypeCommand.Settings>
{
    public sealed class Settings : TypeCodeCommandSettings
    {
        [Description("Typenames for which the tables are generated.")]
        [CommandArgument(0, "[TypeNames]")]
        public string[] TypeNames { get; set; } = null!;
    }

    protected override async Task RunAsync(TypeCodeConsoleServiceProvider serviceProvider, CommandContext context, Settings settings)
    {
        var mode = serviceProvider.GetService<ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter>>();
        var typeProvider = await serviceProvider.GetService<ILazyTypeProviderFactory>().ValueAsync().ConfigureAwait(false);
        var types = typeProvider.TryGetByNames(settings.TypeNames);
        var parameter = new UnitTestDependencyTypeGeneratorParameter { Types = types.ToList() };
        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
    }
}