using System.Diagnostics.CodeAnalysis;
using Autofac;
using Cocona;
using JetBrains.Annotations;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Boot.Helper;

namespace TypeCode.Console.Commands.Builder;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class BuilderCommand
{
    [UsedImplicitly]
    public async Task ExecuteAsync([Argument]string typeName)
    {
        await using (var scope = LifeTimeScopeCreator.BeginLifetimeScope(ContextProvider.Get()))
        {
            var mode = scope.Resolve<ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter>>();
            var typeProvider = scope.Resolve<ITypeProvider>();
            var type = typeProvider.TryGetByName(typeName);
            var parameter = new BuilderTypeCodeGeneratorParameter { Types = type};
            System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        }
    }
}