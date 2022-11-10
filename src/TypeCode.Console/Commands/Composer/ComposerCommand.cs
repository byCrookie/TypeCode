using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Composer;

internal sealed class ComposerCommand : TypeCodeCommand<ComposerCommand.Settings>
{
    public sealed class Settings : TypeCodeCommandSettings
    {
        [Description("Type for which the builder is generated.")]
        [CommandArgument(0, "[TypeName]")]
        public string TypeName { get; set; } = null!;
    }

    protected override async Task RunAsync(TypeCodeConsoleServiceProvider serviceProvider, CommandContext context, Settings settings)
    {
        var mode = serviceProvider.GetService<ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter>>();
        var typeProvider = await serviceProvider.GetService<ILazyTypeProviderFactory>().ValueAsync().ConfigureAwait(false);
        var types = typeProvider.TryGetByName(settings.TypeName);
        var parameter = new ComposerTypeCodeGeneratorParameter();

        foreach (var type in types)
        {
            var interfaces = typeProvider.TryGetTypesByCondition(typ => typ.GetInterface(type.Name) != null).ToList();
            var composerType = new ComposerType(type, interfaces);
            parameter.ComposerTypes.Add(composerType);
        }

        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
    }
}