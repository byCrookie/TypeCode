using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeCode.Business.Format;

namespace TypeCode.Business.Mode.Composer;

internal class ComposerTypeCodeGenerator : IComposerTypeCodeGenerator
{
    public Task<string> GenerateAsync(ComposerTypeCodeGeneratorParameter parameter)
    {
        if (parameter.Type is not null && parameter.Interfaces.Any())
        {
            return Task.FromResult(GenerateComposerCode(parameter.Type, parameter.Interfaces)); 
        }

        return Task.FromResult<string>(null);
    }

    private static string GenerateComposerCode(Type type, List<Type> interfaces)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($@"{Cuts.Long()}");
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
        stringBuilder.AppendLine($@"{Cuts.Long()}");

        return stringBuilder.ToString();
    }
}