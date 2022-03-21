using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Spectre.Console.Cli;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;

namespace TypeCode.Console.Commands.Unit;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class UnitManuallyCommand  : AsyncCommand<UnitManuallyCommand.Settings>
{
    public class Settings : CommandSettings
    {
        public Settings()
        {
            Content = string.Empty;
        }
        
        [UsedImplicitly]
        [Description("Constructor-declaration for which the code is generated.")]
        [CommandArgument(0, "[ConstructorDeclaration]")]
        public string Content { get; init; }
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