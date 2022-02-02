using TypeCode.Business.Mode.Mapper.Style;

namespace TypeCode.Business.Mode.Mapper;

public class MapperTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public MapperTypeCodeGeneratorParameter(MappingType mapFrom, MappingType mapTo)
    {
        MapFrom = mapFrom;
        MapTo = mapTo;
    }
    
    public MappingType MapFrom { get; set; }
    public MappingType MapTo { get; set; }
    public MappingStyle MappingStyle { get; set; }
}