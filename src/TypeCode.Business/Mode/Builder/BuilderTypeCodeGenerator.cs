using System.Reflection;
using System.Text;
using TypeCode.Business.Format;
using TypeCode.Business.TypeEvaluation.Property;

namespace TypeCode.Business.Mode.Builder;

public class BuilderTypeCodeGenerator : IBuilderTypeCodeGenerator
{
    public Task<string?> GenerateAsync(BuilderTypeCodeGeneratorParameter parameter)
    {
        return parameter.Type is not null
            ? Task.FromResult(GenerateBuildersCode(parameter))
            : Task.FromResult<string?>(null);
    }

    private static string? GenerateBuildersCode(BuilderTypeCodeGeneratorParameter parameter)
    {
        if (parameter.AlreadyMapped.Contains(parameter.Type!))
        {
            return null;
        }

        parameter.AlreadyMapped.Add(parameter.Type!);
        
        var code = new StringBuilder();
        
        code.AppendLine(GenerateBuilderCode(parameter.Type!));

        if (parameter.Recursive)
        {
            var complexProperties = parameter.Type!.GetProperties()
                .Where(property => !PropertyEval.IsList(property.PropertyType) 
                                   && !PropertyEval.IsSimple(property.PropertyType));

            foreach (var property in complexProperties)
            {
                parameter.Type = property.PropertyType;
                code.AppendLine(GenerateBuildersCode(parameter));
            }
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
        var dataType = PropertyEval.GetNestedPropertyTypeNameIfAvailable(property.PropertyType);

        if (PropertyEval.IsSimple(property.PropertyType) || property.PropertyType.IsEnum)
        {
            stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder {property.Name}({dataType} value)");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{property.Name} = value;");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
            stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
        }
        else if (PropertyEval.IsList(property.PropertyType) && PropertyEval.IsSimple(PropertyEval.GetNestedTypeIfAvailable(property.PropertyType)))
        {
            stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder Add{property.Name}({dataType} value)");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{property.Name}.Add(value);");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
            stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
        }
        else if (PropertyEval.IsList(property.PropertyType) && !PropertyEval.IsSimple(PropertyEval.GetNestedTypeIfAvailable(property.PropertyType)))
        {
            stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder Add{property.Name}(Action<{PropertyEval.GetNestedTypeIfAvailable(property.PropertyType).Name}Builder> configure)");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}var builder = new {PropertyEval.GetNestedTypeIfAvailable(property.PropertyType).Name}Builder();");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}configure(builder);");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{property.Name}.Add(builder.Build());");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
            stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
        }
        else
        {
            stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder {property.Name}(Action<{property.PropertyType.Name}Builder> configure)");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}var builder = new {property.PropertyType.Name}Builder();");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}configure(builder);");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{property.PropertyType.Name} = builder.Build();");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
            stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
        }
    }
}