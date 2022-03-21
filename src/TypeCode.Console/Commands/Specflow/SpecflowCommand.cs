using System.Diagnostics.CodeAnalysis;
using Cocona;
using JetBrains.Annotations;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Specflow;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class SpecflowCommand
{
    [UsedImplicitly]
    public async Task ExecuteAsync(
        [Option] bool onlyRequired,
        [Option] bool sortAlphabetically,
        [Argument] string[] typeNames,
        [Option] bool includeStrings = true
    )
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var mode = serviceProvider.GetService<ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter>>();
        var typeProvider = serviceProvider.GetService<ITypeProvider>();
        var types = typeProvider.TryGetByNames(typeNames);
        var parameter = new SpecflowTypeCodeGeneratorParameter
        {
            Types = types.ToList(),
            OnlyRequired = onlyRequired,
            SortAlphabetically = sortAlphabetically,
            IncludeStrings = includeStrings
        };
        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
    }
}