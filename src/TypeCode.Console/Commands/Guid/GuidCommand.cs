using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Guid;

namespace TypeCode.Console.Commands.Guid;

internal sealed class GuidCommand : TypeCodeCommand<GuidCommand.Settings>
{
    public sealed class Settings : TypeCodeCommandSettings
    {
        [Description("Type for which the builder is generated.")]
        [CommandOption("-f|--format")]
        [DefaultValue(GuidFormat.D)]
        public GuidFormat GuidFormat { get; set; } = GuidFormat.D;
    }

    protected override async Task RunAsync(TypeCodeConsoleServiceProvider serviceProvider, CommandContext context, Settings settings)
    {
        var mode = serviceProvider.GetService<ITypeCodeGenerator<GuidTypeCodeGeneratorParameter>>();
        var parameter = new GuidTypeCodeGeneratorParameter(settings.GuidFormat);
        System.Console.WriteLine(await mode!.GenerateAsync(parameter).ConfigureAwait(false));
    }
}