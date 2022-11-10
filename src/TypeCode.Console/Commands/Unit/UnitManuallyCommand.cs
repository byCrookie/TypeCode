using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;

namespace TypeCode.Console.Commands.Unit;

internal sealed class UnitManuallyCommand  : TypeCodeCommand<UnitManuallyCommand.Settings>
{
    public sealed class Settings : TypeCodeCommandSettings
    {
        [Description("Constructor-declaration for which the code is generated.")]
        [CommandArgument(0, "[ConstructorDeclaration]")]
        public string Content { get; set; } = null!;
    }

    protected override async Task RunAsync(TypeCodeConsoleServiceProvider serviceProvider, CommandContext context, Settings settings)
    {
        var mode = serviceProvider.GetService<ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter>>();
        var parameter = new UnitTestDependencyManuallyGeneratorParameter { Input = settings.Content };
        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
    }
}