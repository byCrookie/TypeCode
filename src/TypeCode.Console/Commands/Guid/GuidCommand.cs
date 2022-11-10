using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Guid;

namespace TypeCode.Console.Commands.Guid;

public sealed class GuidCommand : AsyncCommand<GuidCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        public Settings(GuidFormat guidFormat)
        {
            GuidFormat = guidFormat;
        }
        
        [Description("Type for which the builder is generated.")]
        [CommandOption("-f|--format")]
        [DefaultValue(GuidFormat.D)]
        public GuidFormat GuidFormat { get; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var mode = serviceProvider.GetService<ITypeCodeGenerator<GuidTypeCodeGeneratorParameter>>();
        
        var parameter = new GuidTypeCodeGeneratorParameter(settings.GuidFormat);
        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));

        return 0;
    }
}