using System.ComponentModel;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;

namespace TypeCode.Console.Commands.Unit;

public class UnitManuallyCommand  : AsyncCommand<UnitManuallyCommand.Settings>
{
    public class Settings : CommandSettings
    {
        public Settings(string content)
        {
            Content = content;
        }
        
        [Description("Constructor-declaration for which the code is generated.")]
        [CommandArgument(0, "[ConstructorDeclaration]")]
        public string Content { get; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var serviceProvider = new TypeCodeConsoleServiceProvider();
        var mode = serviceProvider.GetService<ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter>>();
        var parameter = new UnitTestDependencyManuallyGeneratorParameter { Input = settings.Content };
        System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        return 1;
    }
}