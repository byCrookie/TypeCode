using System.ComponentModel;
using JetBrains.Annotations;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Builder;

public class BuilderCommand : AsyncCommand<BuilderCommand.Settings>
{
    public class Settings : CommandSettings
    {
        public Settings(string typeName)
        {
            TypeName = typeName;
        }
        
        [UsedImplicitly]
        [Description("Type for which the builder is generated.")]
        [CommandArgument(0, "[TypeName]")]
        public string TypeName { get; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var mode = serviceProvider.GetService<ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter>>();
        var typeProvider = serviceProvider.GetService<ITypeProvider>();
        var type = typeProvider.TryGetByName(settings.TypeName);
        var parameter = new BuilderTypeCodeGeneratorParameter { Types = type };
        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        return 0;
    }
}