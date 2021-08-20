using System;
using System.Collections.Generic;
using System.Linq;
using TypeCode.Business.StringProximity;

namespace TypeCode.Business.Mode.Mapper
{
    internal class MappingType
    {
        public Type Type { get; set; }

        public string MostAccurateProperty(string property)
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
                select new ProximityProperty {Property = mappingProperty, Jaro = proximity};

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

            return IsEntity() ? "entity" : "TODO";
        }

        public static string EvaluateMethodName(MappingType secondType)
        {
            if (secondType.IsDto())
            {
                return "MapToDto";
            }

            if (secondType.IsData())
            {
                return "MapToData";
            }

            return secondType.IsEntity() ? "MapToEntity" : "TODO";
        }

        public string DataType()
        {
            if (IsData())
            {
                return "data";
            }

            if (IsDto())
            {
                return "dto";
            }

            return IsEntity() ? "entity" : string.Empty;
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
}