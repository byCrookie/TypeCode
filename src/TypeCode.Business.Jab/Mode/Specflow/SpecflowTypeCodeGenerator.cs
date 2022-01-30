using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Framework.Extensions.Number;
using TypeCode.Business.Format;
using TypeCode.Business.TypeEvaluation.Property;

namespace TypeCode.Business.Mode.Specflow;

internal class SpecflowTypeCodeGenerator : ISpecflowTypeCodeGenerator
{
    public Task<string> GenerateAsync(SpecflowTypeCodeGeneratorParameter parameter)
    {
        return parameter.Types.Any() 
            ? Task.FromResult(GenerateTable(parameter.Types)) 
            : Task.FromResult<string>(null);
    }

    private static string GenerateTable(IEnumerable<Type> types)
    {
        var stringBuilder = new StringBuilder();

        var tables = CreateTables(types);

        foreach (var (key, (header, defaultRow)) in tables)
        {
            stringBuilder.AppendLine($@"{Cuts.Medium()}");
            stringBuilder.AppendLine($@"{Cuts.Heading()} {key}");
            stringBuilder.AppendLine(header);
            stringBuilder.AppendLine(defaultRow);
            stringBuilder.AppendLine($@"{Cuts.Medium()}");
        }

        return stringBuilder.ToString();
    }

    private static IDictionary<Type, (string, string)> CreateTables(IEnumerable<Type> entityTypes)
    {
        var tables = new Dictionary<Type, (string, string)>();

        foreach (var type in entityTypes)
        {
            var properties = RetrieveProperties(type).ToList();
                
            if (properties.Any())
            {
                CreateTableForType(properties, tables, type);
            }
        }

        return tables;
    }

    private static void CreateTableForType(IReadOnlyCollection<KeyValuePair<PropertyInfo, string>> properties, IDictionary<Type, (string, string)> tables, Type type)
    {
        var header = string.Concat(new List<string>
        {
            "| ",
            "#",
            " | ",
            string.Join(" | ", properties.Select(property => property.Value)),
            " |"
        });

        var defaultRow = string.Concat(new List<string>
        {
            "| ",
            $"{string.Concat(type.Name.Where(char.IsUpper))}1",
            " | ",
            string.Join(" | ", properties.Select(GetDefault).ToList()),
            " |"
        });

        tables.Add(type, (header, defaultRow));
    }

    private static string GetDefault(KeyValuePair<PropertyInfo, string> propertyInfo)
    {
        const string defaultNull = "%NULL%";

        var propertyType = propertyInfo.Key.PropertyType;

        if (propertyType.IsEnum)
        {
            return Enum.GetValues(propertyType).GetValue(0)?.ToString();
        }

        if (propertyType == typeof(DateTime))
        {
            var now = DateTime.Now;
            var month = now.Month.IsBetweenInclusive(1, 9) ? $"0{now.Month}" : $"{now.Month}";
            var day = now.Day.IsBetweenInclusive(1, 9) ? $"0{now.Day}" : $"{now.Day}";
            return $"{now.Year}-{month}-{day}";
        }

        if (propertyType == typeof(bool))
        {
            return "false";
        }

        var property = Type.GetType(propertyType.FullName!);

        if (property is null)
        {
            return defaultNull;
        }

        return property.IsValueType ? Activator.CreateInstance(property)?.ToString() : defaultNull;
    }

    private static IEnumerable<KeyValuePair<PropertyInfo, string>> RetrieveProperties(Type type)
    {
        var properties = type.GetProperties()
            .Where(ShouldUseProperty)
            .OrderByDescending(property => property.Name == "Id")
            .ThenByDescending(property => property.Name.Contains("Id"))
            .ThenBy(property => property.Name);

        foreach (var property in properties)
        {
            yield return new KeyValuePair<PropertyInfo, string>(property, CreateHeader(property));
        }
    }

    private static bool ShouldUseProperty(PropertyInfo property)
    {
        return Nullable.GetUnderlyingType(property.PropertyType) == null
               && property.Name != "ModifiedDate"
               && !PropertyEval.IsList(property.PropertyType)
               && property.GetSetMethod() != null
               && property.Name != "Id";
    }

    private static string CreateHeader(PropertyInfo property)
    {
        if (property.Name.Contains("Id"))
        {
            return property.Name == "Id" ? string.Empty : $"#:Id->{property.Name}";
        }

        if (property.PropertyType.IsClass && !PropertyEval.IsSimple(property.PropertyType))
        {
            return $"#:{property.Name}";
        }

        return property.Name;
    }
}