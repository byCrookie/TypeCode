using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Specflow;

public class SpecflowCommand : AsyncCommand<SpecflowCommand.Settings>
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
        
        [Description("Only use required properties for specflow execution.")]
        [CommandOption("-o|--only-required")]
        [DefaultValue(false)]
        public bool OnyRequired { get; init; }
        
        [Description("Sort the table columns alphabetically.")]
        [CommandOption("-s|--sort-alphabetically")]
        [DefaultValue(false)]
        public bool SortAlphabetically { get; init; }
        
        [Description("Include properties of type string.")]
        [CommandOption("-i|--include-strings")]
        [DefaultValue(true)]
        public bool IncludeStrings { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var mode = serviceProvider.GetService<ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter>>();
        var typeProvider = serviceProvider.GetService<ITypeProvider>();
        var types = typeProvider.TryGetByNames(settings.TypeNames);
        var parameter = new SpecflowTypeCodeGeneratorParameter
        {
            Types = types.ToList(),
            OnlyRequired = settings.OnyRequired,
            SortAlphabetically = settings.SortAlphabetically,
            IncludeStrings = settings.IncludeStrings
        };
        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        return 0;
    }
}