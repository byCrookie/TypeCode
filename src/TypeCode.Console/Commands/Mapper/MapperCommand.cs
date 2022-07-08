using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Format;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Mapper;

public class MapperCommand : AsyncCommand<MapperCommand.Settings>
{
    public class Settings : CommandSettings
    {
        public Settings(string from, string to)
        {
            From = from;
            To = to;
        }
        
        [Description("Type for left side of mapping.")]
        [CommandOption("-f|--from")]
        public string From { get; }
        
        [Description("Type for right side of mapping.")]
        [CommandOption("-t|--to")]
        public string To { get; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var mode = serviceProvider.GetService<ITypeCodeGenerator<MapperTypeCodeGeneratorParameter>>();
        var typeProvider = serviceProvider.GetService<ITypeProvider>();
        var fromTypes = typeProvider.TryGetByName(settings.From).ToList();
        var toTypes = typeProvider.TryGetByName(settings.To).ToList();

        if (fromTypes.Count > 1)
        {
            System.Console.WriteLine($@"{Cuts.Medium()} From {settings.From}");
            System.Console.WriteLine($@"To many types found for name {settings.From}. Call the command again and specify the types fullname.");
            fromTypes.ForEach(type => System.Console.WriteLine($@"{Cuts.Short()} {NameBuilder.GetNameWithNamespace(type)}"));
        }

        if (toTypes.Count > 1)
        {
            System.Console.WriteLine($@"{Cuts.Medium()} To {settings.To}");
            System.Console.WriteLine($@"To many types found for name {settings.To}. Call the command again and specify the types fullname.");
            toTypes.ForEach(type => System.Console.WriteLine($@"{Cuts.Short()} {NameBuilder.GetNameWithNamespace(type)}"));
        }

        if (fromTypes.Count == 1 && toTypes.Count == 1)
        {
            var parameter = new MapperTypeCodeGeneratorParameter(new MappingType(fromTypes.First()), new MappingType(toTypes.First()));
            System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        }

        return 0;
    }
}