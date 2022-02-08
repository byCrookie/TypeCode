using System.Diagnostics.CodeAnalysis;
using Autofac;
using Cocona;
using JetBrains.Annotations;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Console.Boot.Helper;

namespace TypeCode.Console.Commands.Unit;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class UnitManuallyCommand
{
    [Command("Input", Aliases = new []{"i"})]
    [UsedImplicitly]
    public async Task InputAsync([Argument]string content)
    {
        await using (var scope = LifeTimeScopeCreator.BeginLifetimeScope(ContextProvider.Get()))
        {
            var mode = scope.Resolve<ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter>>();
            var parameter = new UnitTestDependencyManuallyGeneratorParameter { Input = content };
            System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        }
    }
    
    [Command("Path", Aliases = new []{"p"})]
    [UsedImplicitly]
    public async Task PathAsync([Argument]string content)
    {
        await using (var scope = LifeTimeScopeCreator.BeginLifetimeScope(ContextProvider.Get()))
        {
            var mode = scope.Resolve<ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter>>();
            if (!File.Exists(content)) throw new FileNotFoundException();
            var input = await File.ReadAllTextAsync(content).ConfigureAwait(false);
            var parameter = new UnitTestDependencyManuallyGeneratorParameter { Input = input };
            System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        }
    }
}