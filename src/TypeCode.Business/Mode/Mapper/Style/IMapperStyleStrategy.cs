namespace TypeCode.Business.Mode.Mapper.Style;

internal interface IMapperStyleStrategy
{
    bool IsResponsibleFor(MappingStyle style);
    string Generate(MapperTypeCodeGeneratorParameter parameter);
}