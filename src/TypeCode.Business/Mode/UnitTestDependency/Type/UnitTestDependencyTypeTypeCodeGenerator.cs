using System.Text;
using TypeCode.Business.Format;

namespace TypeCode.Business.Mode.UnitTestDependency.Type;

internal class UnitTestDependencyTypeTypeCodeGenerator : IUnitTestDependencyTypeTypeCodeGenerator
{
    public Task<string?> GenerateAsync(UnitTestDependencyTypeGeneratorParameter parameter)
    {
        return parameter.Types.Any() 
            ? GenerateUnitTestDependenciesAsync(parameter.Types) 
            : Task.FromResult<string?>(null);
    }

    private static Task<string?> GenerateUnitTestDependenciesAsync(List<System.Type> types)
    {
        var stringBuilder = new StringBuilder();

        foreach (var type in types)
        {
            stringBuilder.AppendLine(GenerateDependencies(type));
        }

        return Task.FromResult<string?>(stringBuilder.ToString());
    }

    private static string GenerateDependencies(System.Type type)
    {
        var constructor = type.GetConstructors().OrderByDescending(ctor => ctor.GetParameters()).First();
            
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($@"{Cuts.Long()}");
        stringBuilder.AppendLine($@"{Cuts.Heading()} {constructor.DeclaringType?.FullName}");

        stringBuilder.AppendLine();
        stringBuilder.AppendLine($@"private {constructor.DeclaringType?.Name} _testee;");
        stringBuilder.AppendLine();

        foreach (var param in constructor.GetParameters())
        {
            stringBuilder.AppendLine(
                $@"private {NameBuilder.GetInterfaceName(param.ParameterType)} _{param.Name};");
        }

        stringBuilder.AppendLine();

        stringBuilder.AppendLine("[TestInitialize]");
        stringBuilder.AppendLine("public void TestInitialize()");
        stringBuilder.AppendLine("{");

        foreach (var param in constructor.GetParameters())
        {
            stringBuilder.AppendLine(
                $@"{Cuts.Tab()}_{param.Name} = A.Fake<{NameBuilder.GetInterfaceName(param.ParameterType)}>();");
        }

        stringBuilder.AppendLine();

        stringBuilder.AppendLine($"{Cuts.Tab()}_testee = new {type.Name}(");
        foreach (var param in constructor.GetParameters())
        {
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}_{param.Name},");
        }

        RemoveLastComma(stringBuilder);

        stringBuilder.AppendLine();

        stringBuilder.AppendLine($"{Cuts.Tab()});");
        stringBuilder.AppendLine("}");

        stringBuilder.AppendLine();

        stringBuilder.Append($@"{Cuts.Long()}");
        return stringBuilder.ToString();
    }

    private static void RemoveLastComma(StringBuilder stringBuilder)
    {
        stringBuilder.Length--;
        stringBuilder.Length--;
        stringBuilder.Length--;
    }
}