using System.Diagnostics.CodeAnalysis;
using Cocona;
using JetBrains.Annotations;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Builder;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class BuilderCommand
{
    [UsedImplicitly]
    public async Task ExecuteAsync([Argument] string typeName)
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var mode = serviceProvider.GetService<ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter>>();
        var typeProvider = serviceProvider.GetService<ITypeProvider>();
        var type = typeProvider.TryGetByName(typeName);
        var parameter = new BuilderTypeCodeGeneratorParameter { Types = type };
        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
    }
}