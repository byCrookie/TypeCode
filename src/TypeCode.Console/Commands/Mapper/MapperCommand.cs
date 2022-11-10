using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Format;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Console.Commands.Mapper;

internal sealed class MapperCommand : TypeCodeCommand<MapperCommand.Settings>
{
    public sealed class Settings : TypeCodeCommandSettings
    {
        [Description("Type for left side of mapping.")]
        [CommandOption("-f|--from")]
        public string From { get; set; } = null!;

        [Description("Type for right side of mapping.")]
        [CommandOption("-t|--to")]
        public string To { get; set; } = null!;
    }

    protected override async Task RunAsync(TypeCodeConsoleServiceProvider serviceProvider, CommandContext context, Settings settings)
    {
        var mode = serviceProvider.GetService<ITypeCodeGenerator<MapperTypeCodeGeneratorParameter>>();
        var typeProvider = await serviceProvider.GetService<ILazyTypeProviderFactory>().ValueAsync().ConfigureAwait(false);
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
    }
}