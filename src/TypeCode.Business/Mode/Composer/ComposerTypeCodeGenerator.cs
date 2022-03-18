using System.Text;
using TypeCode.Business.Format;

namespace TypeCode.Business.Mode.Composer;

public class ComposerTypeCodeGenerator : IComposerTypeCodeGenerator
{
    public Task<string?> GenerateAsync(ComposerTypeCodeGeneratorParameter parameter)
    {
        return parameter.ComposerTypes.Any()
            ? Task.FromResult<string?>(GenerateComposers(parameter.ComposerTypes))
            : Task.FromResult<string?>(null);
    }

    private static string GenerateComposers(List<ComposerType> composerTypes)
    {
        var code = new StringBuilder();

        foreach (var composerType in composerTypes)
        {
            code.AppendLine(GenerateComposerCode(composerType.Type, composerType.Interfaces));
        }

        return code.ToString();
    }

    private static string GenerateComposerCode(Type type, List<Type> interfaces)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($@"{Cuts.Heading()} {type.FullName}");

        stringBuilder.AppendLine();
        stringBuilder.AppendLine(@"private IFactory _factory;");
        stringBuilder.AppendLine();

        stringBuilder.AppendLine(@"public Composer(IFactory factory)");
        stringBuilder.AppendLine(@"{");
        stringBuilder.AppendLine($@"{Cuts.Tab()}_factory = factory");
        stringBuilder.AppendLine(@"}");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine($@"public IEnumerable<{NameBuilder.GetNameWithoutGeneric(type)}> Compose()");
        stringBuilder.AppendLine(@"{");
        foreach (var strategyType in interfaces)
        {
            stringBuilder.AppendLine($@"{Cuts.Tab()}yield return _factory.Create<{NameBuilder.GetInterfaceName(strategyType)}>();");
        }

        stringBuilder.AppendLine(@"}");
        stringBuilder.AppendLine();

        return stringBuilder.ToString();
    }
}