namespace TypeCode.Business.Mode.Mapper.Style;

public interface IMapperStyleComposer
{
    IEnumerable<IMapperStyleStrategy> Compose();
}