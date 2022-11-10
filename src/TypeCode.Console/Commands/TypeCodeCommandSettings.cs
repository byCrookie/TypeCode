using System.ComponentModel;
using Spectre.Console.Cli;

namespace TypeCode.Console.Commands;

public abstract class TypeCodeCommandSettings : CommandSettings
{
    [Description("Use the command without a configuration xml and pass path to dlls directly.")]
    [CommandOption("--target-dlls")]
    [DefaultValue(null)]
    public string[]? TargetDlls { get; set; } = null!;
}