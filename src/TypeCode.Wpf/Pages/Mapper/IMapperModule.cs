using Jab;

namespace TypeCode.Wpf.Pages.Mapper;

[ServiceProviderModule]
[Singleton(typeof(MapperView))]
[Singleton(typeof(MapperViewModel))]
public interface IMapperModule
{
    
}