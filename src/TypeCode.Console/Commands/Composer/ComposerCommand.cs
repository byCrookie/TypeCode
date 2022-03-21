using System.Diagnostics.CodeAnalysis;
using Cocona;
using JetBrains.Annotations;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Composer;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class ComposerCommand
{
    [UsedImplicitly]
    public async Task ExecuteAsync([Argument] string typeName)
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var mode = serviceProvider.GetService<ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter>>();
        var typeProvider = serviceProvider.GetService<ITypeProvider>();
        var types = typeProvider.TryGetByName(typeName);
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