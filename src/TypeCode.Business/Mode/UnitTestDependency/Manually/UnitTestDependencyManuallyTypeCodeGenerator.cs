using System.Text;
using System.Text.RegularExpressions;
using TypeCode.Business.Format;

namespace TypeCode.Business.Mode.UnitTestDependency.Manually;

public class UnitTestDependencyManuallyTypeCodeGenerator : IUnitTestDependencyManuallyTypeCodeGenerator
{
    public Task<string?> GenerateAsync(UnitTestDependencyManuallyGeneratorParameter parameter)
    {
        return !string.IsNullOrEmpty(parameter.Input)
            ? GenerateUnitTestDependenciesManuallyAsync(parameter.Input)
            : Task.FromResult<string?>(null);
    }

    private static Task<string?> GenerateUnitTestDependenciesManuallyAsync(string input)
    {
        var lines = input.Split(Environment.NewLine);

        return lines.Length > 1
            ? GenerateForMultiLineAsync(lines)
            : GenerateForSingleLineAsync(lines.Single());
    }

    private static Task<string?> GenerateForMultiLineAsync(IEnumerable<string> lines)
    {
        var singleLine = string.Join("", lines.Select(line => line.Trim().TrimEnd('\r', '\n')));
        return GenerateForSingleLineAsync(singleLine);
    }

    private static readonly Regex PartsRegex =
        new(@"\(([^)]*)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(300));

    private static Task<string?> GenerateForSingleLineAsync(string line)
    {
        var matches = PartsRegex.Split(line);

        if (matches.Length < 2)
        {
            throw new FormatException("Not valid constructor pattern");
        }
            
        var accessorAndName = matches[0];
        var dependencies = matches[1];

        var className = accessorAndName.Split(" ").Skip(1).Single();
        var dependenciesCommaSeperated = dependencies.Split(",").Select(dependency => dependency.Trim());
        var dependenciesByTypeAndName = dependenciesCommaSeperated
            .Select(dependency => new DependencyManually(dependency))
            .ToList();

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($@"{Cuts.Heading()} {className}");

        stringBuilder.AppendLine();
        stringBuilder.AppendLine($@"private {className} _testee;");
        stringBuilder.AppendLine();

        foreach (var dependency in dependenciesByTypeAndName)
        {
            stringBuilder.AppendLine(
                $@"private {dependency.TypeName} _{dependency.Name};");
        }

        stringBuilder.AppendLine();

        stringBuilder.AppendLine("[TestInitialize]");
        stringBuilder.AppendLine("public void TestInitialize()");
        stringBuilder.AppendLine("{");

        foreach (var dependency in dependenciesByTypeAndName)
        {
            stringBuilder.AppendLine(
                $@"{Cuts.Tab()}_{dependency.Name} = A.Fake<{dependency.TypeName}>();");
        }

        stringBuilder.AppendLine();

        stringBuilder.AppendLine($"{Cuts.Tab()}_testee = new {className}(");
        foreach (var dependency in dependenciesByTypeAndName)
        {
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}_{dependency.Name},");
        }

        RemoveLastComma(stringBuilder);

        stringBuilder.AppendLine();

        stringBuilder.AppendLine($"{Cuts.Tab()});");
        stringBuilder.AppendLine("}");

        stringBuilder.AppendLine();
        
        return Task.FromResult<string?>(stringBuilder.ToString());
    }

    private static void RemoveLastComma(StringBuilder stringBuilder)
    {
        stringBuilder.Length--;
        stringBuilder.Length--;
        stringBuilder.Length--;
    }
}