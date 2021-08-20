using System.Collections.Generic;
using System.Linq;
using Framework.Autofac.Factory;

namespace TypeCode.Business.Mode.Mapper.Style
{
    internal class MapperStyleComposer : IMapperStyleComposer
    {
        private readonly IFactory _factory;

        public MapperStyleComposer(IFactory factory)
        {
            _factory = factory;
        }
		
        public IEnumerable<IMapperStyleStrategy> ComposeOrdered()
        {
            return Compose().OrderBy(mode => mode.Number()).ToList();
        }
		
        private IEnumerable<IMapperStyleStrategy> Compose()
        {
            yield return _factory.Create<IExistingMapperStyleStrategy>();
            yield return _factory.Create<INewMapperStyleStrategy>();
        }
    }
}