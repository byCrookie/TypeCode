using Jab;
using TypeCode.Wpf.Components.InputBox;

namespace TypeCode.Wpf.Components;

[ServiceProviderModule]
[Transient(typeof(IInputBoxViewModelFactory), typeof(InputBoxViewModelFactory))]
public interface IComponentsModule
{
    
}