using Jab;
using TypeCode.Wpf.Components.SearchBox;

namespace TypeCode.Wpf.Components;

[ServiceProviderModule]
[Transient(typeof(ISearchBoxViewModelFactory), typeof(SearchBoxViewModelFactory))]
public interface IComponentsModule
{
    
}