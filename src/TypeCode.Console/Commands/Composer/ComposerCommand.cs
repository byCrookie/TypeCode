using System.ComponentModel;
using JetBrains.Annotations;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Composer;

public class ComposerCommand : AsyncCommand<ComposerCommand.Settings>
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
        var mode = serviceProvider.GetService<ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter>>();
        var typeProvider = serviceProvider.GetService<ITypeProvider>();
        var types = typeProvider.TryGetByName(settings.TypeName);
        var parameter = new ComposerTypeCodeGeneratorParameter();

        foreach (var type in types)
        {
            var interfaces = typeProvider.TryGetTypesByCondition(typ => typ.GetInterface(type.Name) != null).ToList();
            var composerType = new ComposerType(type, interfaces);
            parameter.ComposerTypes.Add(composerType);
        }

        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        return 0;
    }
}