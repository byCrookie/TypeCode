using System.ComponentModel;
using Spectre.Console.Cli;

namespace TypeCode.Console.Commands;

public abstract class TypeCodeCommandSettings : CommandSettings
{
    [Description("Use the command without a configuration xml and pass paths to dlls directly seperated by ' '.")]
    [CommandOption("--dll-paths")]
    [DefaultValue(null)]
    public string[]? DllPaths { get; set; } = null!;
    
    [Description("Depends on --dll-paths. Define which pattern is used to load dlls (Test.*.dll).")]
    [CommandOption("--dll-pattern")]
    [DefaultValue("*.dll")]
    public string DllPattern { get; set; } = null!;

    [Description("Depends on --dll-paths. Load recursivly in all directories.")]
    [CommandOption("--dll-deep")]
    [DefaultValue(false)]
    public bool DllDeep { get; set; } = false;
}