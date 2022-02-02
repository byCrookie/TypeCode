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

    public string? MostAccurateProperty(string property)
    {
        var properties = Properties().ToList();

        var equalProperty = properties.SingleOrDefault(prop => string.Equals(prop, property, StringComparison.CurrentCultureIgnoreCase));

        if (equalProperty != null)
        {
            return equalProperty;
        }

        var propertiesWithProximities =
            from mappingProperty in properties
            let proximity = JaroWinklerDistance.Proximity(mappingProperty, property)
            select new ProximityProperty(mappingProperty, proximity);

        return propertiesWithProximities
            .OrderByDescending(distance => distance.Jaro)
            .FirstOrDefault(dist => dist.Jaro >= 0.8)?.Property;
    }

    public IEnumerable<string> Properties()
    {
        return Type
            .GetProperties()
            .Where(property =>
                property.PropertyType.IsPublic
                && property.SetMethod != null)
            .Select(prop => prop.Name);
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

        return IsEntity() ? "entity" : Type.Name.Camelize();
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