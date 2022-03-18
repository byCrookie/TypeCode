using Jab;

namespace TypeCode.Wpf.Pages.Mapper;

[ServiceProviderModule]
[Transient(typeof(MapperView))]
[Transient(typeof(MapperViewModel))]
public interface IMapperModule
{
    
}