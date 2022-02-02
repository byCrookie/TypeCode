namespace TypeCode.Business.Mode.Mapper.Style;

internal interface IMapperStyleComposer
{
    IEnumerable<IMapperStyleStrategy> Compose();
}