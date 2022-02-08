using System.Diagnostics.CodeAnalysis;
using Autofac;
using Cocona;
using JetBrains.Annotations;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Boot.Helper;

namespace TypeCode.Console.Commands.Unit;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class UnitTypeCommand
{
    [UsedImplicitly]
    public async Task ExecuteAsync([Argument]string[] typeNames)
    {
        await using (var scope = LifeTimeScopeCreator.BeginLifetimeScope(ContextProvider.Get()))
        {
            var mode = scope.Resolve<ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter>>();
            var typeProvider = scope.Resolve<ITypeProvider>();
            var types = typeProvider.TryGetByNames(typeNames);
            var parameter = new UnitTestDependencyTypeGeneratorParameter { Types = types.ToList() };
            System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        }
    }
}