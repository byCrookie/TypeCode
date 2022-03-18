using Framework.DependencyInjection.Factory;

namespace TypeCode.Business.Mode.Mapper.Style;

public class MapperStyleComposer : IMapperStyleComposer
{
    private readonly IFactory _factory;

    public MapperStyleComposer(IFactory factory)
    {
        _factory = factory;
    }
        
    public IEnumerable<IMapperStyleStrategy> Compose()
    {
        yield return _factory.Create<IExistingMapperStyleStrategy>();
        yield return _factory.Create<INewMapperStyleStrategy>();
    }
}