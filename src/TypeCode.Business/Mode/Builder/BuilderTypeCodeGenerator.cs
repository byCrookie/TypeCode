using System.Reflection;
using System.Text;
using TypeCode.Business.Format;
using TypeCode.Business.TypeEvaluation.Property;

namespace TypeCode.Business.Mode.Builder;

public class BuilderTypeCodeGenerator : IBuilderTypeCodeGenerator
{
    public Task<string?> GenerateAsync(BuilderTypeCodeGeneratorParameter parameter)
    {
        return parameter.Types.Any()
            ? Task.FromResult<string?>(GenerateBuildersCode(parameter.Types))
            : Task.FromResult<string?>(null);
    }

    private static string GenerateBuildersCode(IEnumerable<Type> types)
    {
        var code = new StringBuilder();

        foreach (var type in types)
        {
            code.AppendLine(GenerateBuilderCode(type));
        }

        return code.ToString();
    }

    private static string GenerateBuilderCode(Type type)
    {
        var stringBuilder = new StringBuilder();
        
        stringBuilder.AppendLine($@"{Cuts.Heading()} {type.FullName}");

        var typeName = NameBuilder.GetNameWithoutGeneric(type);
        var classFieldName = NameBuilder.FieldNameFromClass(type);
        var classVariableName = NameBuilder.VariableNameFromClass(type);

        stringBuilder.AppendLine($"public class {typeName}Builder");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine($@"{Cuts.Tab()}private static {typeName} {classFieldName};");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder()");

        stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
        stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName} = new {typeName}();");
        stringBuilder.AppendLine($@"{Cuts.Tab()}}}");

        foreach (var property in type.GetProperties().Where(p => p.GetSetMethod() != null || p.Name == "Id"))
        {
            stringBuilder.AppendLine();
            CreatePropertyMethod(stringBuilder, typeName, classFieldName, property);
        }

        stringBuilder.AppendLine();
        stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName} Build()");
        stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
        stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}var {classVariableName} = {classFieldName};");
        stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName} = new {typeName}();");
        stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return {classVariableName};");
        stringBuilder.AppendLine($@"{Cuts.Tab()}}}");

        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }

    private static void CreatePropertyMethod(StringBuilder stringBuilder, string typeName, string classFieldName, PropertyInfo property)
    {
        var propertyName = property.Name;
        var dataType = PropertyEval.GetNestedPropertyTypeNameIfAvailable(property.PropertyType);

        if (PropertyEval.IsSimple(property.PropertyType) || property.PropertyType.IsEnum)
        {
            stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder {propertyName}({dataType} value)");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{propertyName} = value;");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
            stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
        }
        else if (PropertyEval.IsList(property.PropertyType) && PropertyEval.IsSimple(PropertyEval.GetNestedTypeIfAvailable(property.PropertyType)))
        {
            stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder Add{propertyName}({dataType} value)");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{propertyName}.Add(value);");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
            stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
        }
        else if (PropertyEval.IsList(property.PropertyType) && !PropertyEval.IsSimple(PropertyEval.GetNestedTypeIfAvailable(property.PropertyType)))
        {
            stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder Add{propertyName}(Action<{propertyName}Builder> configure)");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}var builder = new {propertyName}Builder();");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}configure(builder);");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{propertyName}.Add(builder.Build());");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
            stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
        }
        else
        {
            stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder {propertyName}(Action<{propertyName}Builder> configure)");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}var builder = new {propertyName}Builder();");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}configure(builder);");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{propertyName} = builder.Build();");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
            stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
        }
    }
}