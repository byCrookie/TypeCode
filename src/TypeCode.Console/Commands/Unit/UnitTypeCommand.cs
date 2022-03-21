using System.Diagnostics.CodeAnalysis;
using Cocona;
using JetBrains.Annotations;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Unit;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class UnitTypeCommand
{
    [UsedImplicitly]
    public async Task ExecuteAsync([Argument] string[] typeNames)
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var mode = serviceProvider.GetService<ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter>>();
        var typeProvider = serviceProvider.GetService<ITypeProvider>();
        var types = typeProvider.TryGetByNames(typeNames);
        var parameter = new UnitTestDependencyTypeGeneratorParameter { Types = types.ToList() };
        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
    }
}