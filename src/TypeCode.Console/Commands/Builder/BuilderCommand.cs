using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Builder;

public sealed class BuilderCommand : AsyncCommand<BuilderCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        public Settings(string typeName)
        {
            TypeName = typeName;
        }
        
        [Description("Type for which the builder is generated.")]
        [CommandArgument(0, "[TypeName]")]
        public string TypeName { get; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var mode = serviceProvider.GetService<ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter>>();
        var typeProvider = serviceProvider.GetService<ITypeProvider>();
        var types = typeProvider.TryGetByName(settings.TypeName);

        foreach (var type in types)
        {
            var parameter = new BuilderTypeCodeGeneratorParameter { Type = type };
            System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        }

        return 0;
    }
}