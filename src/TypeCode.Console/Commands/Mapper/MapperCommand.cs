using System.Diagnostics.CodeAnalysis;
using Autofac;
using JetBrains.Annotations;
using TypeCode.Business.Format;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Boot.Helper;

namespace TypeCode.Console.Commands.Mapper;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class MapperCommand
{
    [UsedImplicitly]
    public async Task ExecuteAsync(string from, string to)
    {
        await using (var scope = LifeTimeScopeCreator.BeginLifetimeScope(ContextProvider.Get()))
        {
            var mode = scope.Resolve<ITypeCodeGenerator<MapperTypeCodeGeneratorParameter>>();
            var typeProvider = scope.Resolve<ITypeProvider>();
            var fromTypes = typeProvider.TryGetByName(from).ToList();
            var toTypes = typeProvider.TryGetByName(to).ToList();
            
            if (fromTypes.Count > 1)
            {
                System.Console.WriteLine($@"{Cuts.Medium()} From {from}");
                System.Console.WriteLine($@"To many types found for name {from}. Call the command again and specify the types fullname.");
                fromTypes.ForEach(type => System.Console.WriteLine($@"{Cuts.Short()} {NameBuilder.GetNameWithNamespace(type)}"));
            }
            
            if (toTypes.Count > 1)
            {
                System.Console.WriteLine($@"{Cuts.Medium()} From {to}");
                System.Console.WriteLine($@"To many types found for name {to}. Call the command again and specify the types fullname.");
                toTypes.ForEach(type => System.Console.WriteLine($@"{Cuts.Short()} {NameBuilder.GetNameWithNamespace(type)}"));
            }

            if (fromTypes.Count == 1 && toTypes.Count == 1)
            {
                var parameter = new MapperTypeCodeGeneratorParameter(new MappingType(fromTypes.First()), new MappingType(toTypes.First()));
                System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
            }
        }
    }
}