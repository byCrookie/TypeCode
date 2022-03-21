using TypeCode.Business.Mode.Mapper.Style;

namespace TypeCode.Business.Mode.Mapper;

public class MapperTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public MapperTypeCodeGeneratorParameter(MappingType? mapFrom, MappingType? mapTo)
    {
        MapFrom = mapFrom;
        MapTo = mapTo;
        AlreadyMapped = new List<Type>();
    }
    
    public MappingType? MapFrom { get; set; }
    public MappingType? MapTo { get; set; }
    public MappingStyle MappingStyle { get; set; }
    public bool MapTree { get; set; }
    public bool MapSingleDirectionOnly { get; set; }
    public List<Type> AlreadyMapped { get; set; }
}