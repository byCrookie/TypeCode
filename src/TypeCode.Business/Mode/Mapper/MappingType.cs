using Humanizer;
using TypeCode.Business.StringProximity;

namespace TypeCode.Business.Mode.Mapper;

public class MappingType
{
    public MappingType(Type? type)
    {
        Type = type;
    }

    public Type? Type { get; }

    public TypeCodeProperty? MostAccurateProperty(string property)
    {
        var properties = Properties().ToList();

        var equalProperty = properties.SingleOrDefault(prop => string.Equals(prop.Name, property, StringComparison.CurrentCultureIgnoreCase));

        if (equalProperty != null)
        {
            return equalProperty;
        }

        var propertiesWithProximities =
            from mappingProperty in properties
            let proximity = JaroWinklerDistance.Proximity(mappingProperty.Name, property)
            select new ProximityProperty(mappingProperty, proximity);

        return propertiesWithProximities
            .OrderByDescending(distance => distance.Jaro)
            .FirstOrDefault(dist => dist.Jaro >= 0.8)?.Property;
    }

    public IEnumerable<TypeCodeProperty> Properties()
    {
        return Type?
            .GetProperties()
            .Where(property =>
                property.PropertyType.IsPublic
                && property.SetMethod != null)
            .Select(prop => new TypeCodeProperty(prop.Name, prop)) ?? new List<TypeCodeProperty>();
    }

    public string ParameterName()
    {
        if (IsDto())
        {
            return "dto";
        }

        if (IsData())
        {
            return "data";
        }

        return IsEntity() ? "entity" : Type?.Name.Camelize() ?? "todo";
    }

    public string EvaluateMethodName()
    {
        if (IsDto())
        {
            return "MapToDto";
        }

        if (IsData())
        {
            return "MapToData";
        }

        return IsEntity() ? "MapToEntity" : "MapTo";
    }

    private bool IsEntity()
    {
        return Type is not null && Type.Name.Contains("Entity");
    }

    private bool IsData()
    {
        return Type is not null && Type.Name.Contains("Data");
    }

    private bool IsDto()
    {
        return Type is not null && Type.Name.Contains("Dto");
    }
}