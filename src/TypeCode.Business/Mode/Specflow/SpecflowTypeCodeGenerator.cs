using System.Reflection;
using System.Text;
using Framework.Extensions.Number;
using TypeCode.Business.TypeEvaluation.Property;

namespace TypeCode.Business.Mode.Specflow;

public class SpecflowTypeCodeGenerator : ISpecflowTypeCodeGenerator
{
    public Task<string?> GenerateAsync(SpecflowTypeCodeGeneratorParameter parameter)
    {
        return parameter.Types.Any()
            ? Task.FromResult<string?>(GenerateTable(parameter))
            : Task.FromResult<string?>(null);
    }

    private static string GenerateTable(SpecflowTypeCodeGeneratorParameter parameter)
    {
        var stringBuilder = new StringBuilder();

        var tables = CreateTables(parameter);

        foreach (var (key, (header, defaultRow)) in tables)
        {
            stringBuilder.AppendLine($@"And Entity {key}");
            stringBuilder.AppendLine(header);
            stringBuilder.AppendLine(defaultRow);
            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }

    private static IDictionary<Type, (string, string)> CreateTables(SpecflowTypeCodeGeneratorParameter parameter)
    {
        var tables = new Dictionary<Type, (string, string)>();

        foreach (var type in parameter.Types)
        {
            var properties = RetrieveProperties(parameter, type).ToList();

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
            return Enum.GetValues(propertyType).GetValue(0)?.ToString() ?? defaultNull;
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

        return property.IsValueType ? Activator.CreateInstance(property)?.ToString() ?? defaultNull : defaultNull;
    }

    private static IEnumerable<KeyValuePair<PropertyInfo, string>> RetrieveProperties(SpecflowTypeCodeGeneratorParameter parameter, Type type)
    {
        var properties = type.GetProperties()
            .Where(property => IncludeProperty(parameter, property))
            .OrderByDescending(property => property.Name == "Id")
            .ThenByDescending(property => property.Name.Contains("Id"))
            .ThenByDescending(property => property.PropertyType.IsClass && !PropertyEval.IsSimple(property.PropertyType));

        if (parameter.SortAlphabetically)
        {
            properties = properties
                .ThenBy(prop => prop.Name);
        }

        foreach (var property in properties.ToList())
        {
            yield return new KeyValuePair<PropertyInfo, string>(property, CreateHeader(property));
        }
    }

    private static bool IncludeProperty(SpecflowTypeCodeGeneratorParameter parameter, PropertyInfo property)
    {
        var include = property.Name != "ModifiedDate"
                      && !PropertyEval.IsList(property.PropertyType)
                      && property.GetSetMethod() != null
                      && property.Name != "Id";

        if (parameter.OnlyRequired)
        {
            include = include
                      && !IsNullable(property);
        }

        if (!parameter.IncludeStrings)
        {
            include = include
                      && property.PropertyType != typeof(string);
        }

        return include;
    }

    private static bool IsNullable(PropertyInfo property)
    {
        return Nullable.GetUnderlyingType(property.PropertyType) is not null;
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