using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Specflow;

internal sealed class SpecflowCommand : TypeCodeCommand<SpecflowCommand.Settings>
{
    public sealed class Settings : TypeCodeCommandSettings
    {
        [Description("Typenames for which the tables are generated.")]
        [CommandArgument(0, "[TypeNames]")]
        public string[] TypeNames { get; set; } = null!;
        
        [Description("Only use required properties for specflow execution.")]
        [CommandOption("-o|--only-required")]
        [DefaultValue(false)]
        public bool OnyRequired { get; set; } = false;
        
        [Description("Sort the table columns alphabetically.")]
        [CommandOption("-s|--sort-alphabetically")]
        [DefaultValue(false)]
        public bool SortAlphabetically { get; set; } = false;
        
        [Description("Include properties of type string.")]
        [CommandOption("-i|--include-strings")]
        [DefaultValue(true)]
        public bool IncludeStrings { get; set; } = true;
    }

    protected override async Task RunAsync(TypeCodeConsoleServiceProvider serviceProvider, CommandContext context, Settings settings)
    {
        var mode = serviceProvider.GetService<ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter>>();
        var typeProvider = await serviceProvider.GetService<ILazyTypeProviderFactory>().ValueAsync().ConfigureAwait(false);
        var types = typeProvider.TryGetByNames(settings.TypeNames);
        var parameter = new SpecflowTypeCodeGeneratorParameter
        {
            Types = types.ToList(),
            OnlyRequired = settings.OnyRequired,
            SortAlphabetically = settings.SortAlphabetically,
            IncludeStrings = settings.IncludeStrings
        };
        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
    }
}