using System.Reflection;
using System.Text;
using TypeCode.Business.Format;
using TypeCode.Business.TypeEvaluation.Property;

namespace TypeCode.Business.Mode.Mapper.Style;

public sealed class NewMapperStyleStrategy : INewMapperStyleStrategy
{
    public bool IsResponsibleFor(MappingStyle style)
    {
        return style == MappingStyle.New;
    }

    public string? Generate(MapperTypeCodeGeneratorParameter parameter)
    {
        if (parameter.MapFrom?.Type is null || parameter.MapTo?.Type is null)
        {
            return null;
        }

        if (parameter.AlreadyMapped.Contains(parameter.MapTo.Type))
        {
            return null;
        }

        parameter.AlreadyMapped.Add(parameter.MapTo.Type);

        var firstType = parameter.MapFrom;
        var secondType = parameter.MapTo;

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{Cuts.Heading()} {firstType.Type.Name} -> {secondType.Type.Name}, {firstType.Type.FullName} -> {secondType.Type.FullName}");
        stringBuilder.AppendLine();
        GenerateUsingStyle(stringBuilder, firstType, secondType);
        stringBuilder.AppendLine();

        if (!parameter.SingleDirectionOnly)
        {
            stringBuilder.AppendLine($"{Cuts.Heading()} {secondType.Type.Name} -> {firstType.Type.Name}, {secondType.Type.FullName} -> {firstType.Type.FullName}");
            stringBuilder.AppendLine();
            GenerateUsingStyle(stringBuilder, secondType, firstType);
            stringBuilder.AppendLine();
        }

        if (parameter.Recursiv)
        {
            MapSubClasses(parameter, stringBuilder);
        }

        return stringBuilder.ToString();
    }

    private void MapSubClasses(MapperTypeCodeGeneratorParameter parameter, StringBuilder stringBuilder)
    {
        var listProperties = parameter.MapTo?.Type?.GetProperties().Where(property => PropertyEval.IsList(property.PropertyType));
        var complexProperties = parameter.MapTo?.Type?.GetProperties().Where(property => !PropertyEval.IsList(property.PropertyType) && !PropertyEval.IsSimple(property.PropertyType));

        foreach (var listProperty in listProperties ?? new List<PropertyInfo>())
        {
            if (listProperty.PropertyType.GenericTypeArguments.Length == 1)
            {
                stringBuilder.AppendLine(Generate(new MapperTypeCodeGeneratorParameter(
                    new MappingType(listProperty.PropertyType.GenericTypeArguments.Single()),
                    new MappingType(listProperty.PropertyType.GenericTypeArguments.Single())
                )
                {
                    AlreadyMapped = parameter.AlreadyMapped,
                    Recursiv = parameter.Recursiv,
                    SingleDirectionOnly = parameter.SingleDirectionOnly
                }));
            }
        }

        foreach (var complexProperty in complexProperties ?? new List<PropertyInfo>())
        {
            stringBuilder.AppendLine(Generate(new MapperTypeCodeGeneratorParameter(
                new MappingType(complexProperty.PropertyType),
                new MappingType(complexProperty.PropertyType)
            )
            {
                AlreadyMapped = parameter.AlreadyMapped,
                Recursiv = parameter.Recursiv,
                SingleDirectionOnly = parameter.SingleDirectionOnly
            }));
        }
    }

    private static void GenerateUsingStyle(StringBuilder stringBuilder, MappingType firstType, MappingType secondType)
    {
        if (firstType.Type is null || secondType.Type is null)
        {
            return;
        }

        var methodName = secondType.EvaluateMethodName();

        stringBuilder.AppendLine($"public {secondType.Type.Name} {methodName}({firstType.Type.Name} {firstType.ParameterName()})");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine($"{Cuts.Tab()}return new {secondType.Type.Name}");
        stringBuilder.AppendLine($"{Cuts.Tab()}{{");
        foreach (var property in secondType.Properties())
        {
            if (PropertyEval.IsList(property.Prop.PropertyType))
            {
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{property.Name} = {firstType.ParameterName()}.{firstType.MostAccurateProperty(property.Name)?.Name}.Select({methodName}).ToList(),");
            }
            else
            {
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{property.Name} = {firstType.ParameterName()}.{firstType.MostAccurateProperty(property.Name)?.Name},");
            }
        }

        RemoveLastComma(stringBuilder);

        stringBuilder.AppendLine();

        stringBuilder.AppendLine($"{Cuts.Tab()}}};");

        stringBuilder.AppendLine("}");
    }

    private static void RemoveLastComma(StringBuilder stringBuilder)
    {
        stringBuilder.Length--;
        stringBuilder.Length--;
        stringBuilder.Length--;
    }
}