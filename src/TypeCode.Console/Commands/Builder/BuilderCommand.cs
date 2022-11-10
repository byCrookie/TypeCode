using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Builder;

internal sealed class BuilderCommand : TypeCodeCommand<BuilderCommand.Settings>
{
    public sealed class Settings : TypeCodeCommandSettings
    {
        [Description("Type for which the builder is generated.")]
        [CommandArgument(0, "[TypeName]")]
        public string TypeName { get; set; } = null!;
    }

    protected override async Task RunAsync(TypeCodeConsoleServiceProvider serviceProvider, CommandContext context, Settings settings)
    {
        var mode = serviceProvider.GetService<ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter>>();
        var typeProvider = await serviceProvider.GetService<ILazyTypeProviderFactory>().ValueAsync().ConfigureAwait(false);
        var types = typeProvider.TryGetByName(settings.TypeName);

        foreach (var type in types)
        {
            var parameter = new BuilderTypeCodeGeneratorParameter { Type = type };
            System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        }
    }
}