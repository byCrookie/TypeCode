namespace TypeCode.Business.Mode.Mapper.Style;

public interface IMapperStyleStrategy
{
    bool IsResponsibleFor(MappingStyle style);
    string? Generate(MapperTypeCodeGeneratorParameter parameter);
}